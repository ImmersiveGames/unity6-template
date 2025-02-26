using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests {
    public class Provider : MonoBehaviour, IDependencyProvider {
        [Provide]
        public ServiceA ProvideServiceA() {
            return new ServiceA();
        }

        [Provide]
        public ServiceB ProvideServiceB() {
            return new ServiceB();
        }

        [Provide]
        public FactoryA ProvideFactoryA() {
            return new FactoryA();
        }
    }

    public class ServiceA {
        public void Initialize(string a = null) {
            DebugManager.Log<Provider>($"[ServiceA] Initialize: {a}");
        }
    }
    public class ServiceB {
        public void Initialize(string a) {
            DebugManager.Log<Provider>($"[ServiceB] Initialize: {a}");
        }
    }

    public class FactoryA {
        private ServiceA cacheServiceA;

        public ServiceA CreateServiceA() {
            return cacheServiceA ??= new ServiceA();
        }
    }
}