using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _ImmersiveGames.Scripts.Utils.DebugSystems;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Interface;
using _ImmersiveGames.Scripts.Utils.Singletons;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : PropertyAttribute { }
    
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute { }
    
    [DefaultExecutionOrder(-1000)]
    public class Injector : Singleton<Injector> {
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> registry = new();

        protected override void Awake() {
            base.Awake();
            var monoBehaviours = FindMonoBehaviours();
            
            // Find all modules implementing IDependencyProvider and register the dependencies they provide
            var providers = monoBehaviours.OfType<IDependencyProvider>();
            foreach (var provider in providers) {
                Register(provider);
            }
            
            // Find all injectable objects and inject their dependencies
            var injectables = monoBehaviours.Where(IsInjectable);
            foreach (var injectable in injectables) {
                Inject(injectable);
            }
        }

        // Register an instance of a type outside the normal dependency injection process
        public void Register<T>(T instance) {
            registry[typeof(T)] = instance;
        }

        private void Inject(object instance) {
            var type = instance.GetType();
            
            // Inject into fields
            var injectableFields = type.GetFields(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields) {
                if (injectableField.GetValue(instance) != null) {
                    DebugManager.LogWarning<Injector>($"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null) {
                    throw new Exception($"Failed to inject dependency into field '{injectableField.Name}' of class '{type.Name}'.");
                }
                
                injectableField.SetValue(instance, resolvedInstance);
                DebugManager.Log<Injector>($"Injectable field '{fieldType.Name}' of class '{type.Name}'.");
            }
            
            // Inject into methods
            var injectableMethods = type.GetMethods(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods) {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null)) {
                    throw new Exception($"Failed to inject dependencies into method '{injectableMethod.Name}' of class '{type.Name}'.");
                }
                
                injectableMethod.Invoke(instance, resolvedInstances);
                DebugManager.Log<Injector>($"Injected method '{injectableMethod.Name}' of class '{type.Name}'.");
            }
            
            // Inject into properties
            var injectableProperties = type.GetProperties(BindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties) {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);
                if (resolvedInstance == null) {
                    throw new Exception($"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }

                injectableProperty.SetValue(instance, resolvedInstance);
                DebugManager.Log<Injector>($"Injected property '{propertyType.Name}' of class '{type.Name}'.");
            }
        }

        private void Register(IDependencyProvider provider) {
            var methods = provider.GetType().GetMethods(BindingFlags);

            foreach (var method in methods) {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                
                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);
                if (providedInstance != null) {
                    registry.Add(returnType, providedInstance);
                    DebugManager.Log<Injector>($"Registered provided instance of {returnType.Name} for {method.Name}");
                } else {
                    throw new Exception($"Provider method '{method.Name}' in class '{provider.GetType().Name}' returned null when providing type '{returnType.Name}'.");
                }
            }
        }

        public void ValidateDependencies() {
            var monoBehaviours = FindMonoBehaviours();
            var providers = monoBehaviours.OfType<IDependencyProvider>();
            var providedDependencies = GetProvidedDependencies(providers);

            var invalidDependencies = monoBehaviours
                .SelectMany(mb => mb.GetType().GetFields(BindingFlags), (mb, field) => new {mb, field})
                .Where(t => Attribute.IsDefined(t.field, typeof(InjectAttribute)))
                .Where(t => !providedDependencies.Contains(t.field.FieldType) && t.field.GetValue(t.mb) == null)
                .Select(t => $"[Validation] {t.mb.GetType().Name} is missing dependency {t.field.FieldType.Name} on GameObject {t.mb.gameObject.name}");
            
            var invalidDependencyList = invalidDependencies.ToList();
            
            if (!invalidDependencyList.Any()) {
                DebugManager.Log<Injector>("[Validation] All dependencies are valid.");
            } else {
                DebugManager.LogError<Injector>($"[Validation] {invalidDependencyList.Count} dependencies are invalid:");
                foreach (var invalidDependency in invalidDependencyList) {
                    DebugManager.LogError<Injector>(invalidDependency);
                }
            }
        }

        private HashSet<Type> GetProvidedDependencies(IEnumerable<IDependencyProvider> providers) {
            var providedDependencies = new HashSet<Type>();
            foreach (var provider in providers) {
                var methods = provider.GetType().GetMethods(BindingFlags);
                
                foreach (var method in methods) {
                    if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;
                    
                    var returnType = method.ReturnType;
                    providedDependencies.Add(returnType);
                }
            }

            return providedDependencies;
        }

        public void ClearDependencies() {
            foreach (var monoBehaviour in FindMonoBehaviours()) {
                var type = monoBehaviour.GetType();
                var injectableFields = type.GetFields(BindingFlags)
                    .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

                foreach (var injectableField in injectableFields) {
                    injectableField.SetValue(monoBehaviour, null);
                }
            }
            
            DebugManager.Log<Injector>("All injectable fields cleared.");
        }

        private object Resolve(Type type) {
            registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        private static MonoBehaviour[] FindMonoBehaviours() {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }

        private static bool IsInjectable(MonoBehaviour obj) {
            var members = obj.GetType().GetMembers(BindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }
    }
}