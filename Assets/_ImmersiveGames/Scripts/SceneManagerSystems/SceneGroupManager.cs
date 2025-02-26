using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _ImmersiveGames.Scripts.SceneManagerSystems.Systems.SceneManagement;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    /// <summary>
    /// Gerencia o carregamento e descarregamento de grupos de cenas no jogo.
    /// Suporta tanto cenas regulares quanto Addressables (se disponíveis).
    /// </summary>
    public class SceneGroupManager {
        /// <summary> Evento acionado quando uma cena é carregada. </summary>
        public event Action<string> OnSceneLoaded = delegate { };

        /// <summary> Evento acionado quando uma cena é descarregada. </summary>
        public event Action<string> OnSceneUnloaded = delegate { };

        /// <summary> Evento acionado quando todas as cenas do grupo foram carregadas. </summary>
        public event Action OnSceneGroupLoaded = delegate { };
        
#if UNITY_ADDRESSABLES
        /// <summary> Grupo de handles para controlar operações de carregamento de Addressables. </summary>
        readonly AsyncOperationHandleGroup handleGroup = new AsyncOperationHandleGroup(10);
#endif
        
        /// <summary> Cena ativa no momento. </summary>
        private SceneGroup _activeSceneGroup;
        
        /// <summary>
        /// Carrega um grupo de cenas de forma assíncrona.
        /// </summary>
        /// <param name="group">Grupo de cenas a serem carregadas.</param>
        /// <param name="progress">Progresso do carregamento.</param>
        /// <param name="reloadDupScenes">Se verdadeiro, recarrega cenas duplicadas.</param>
        public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes = false) { 
            _activeSceneGroup = group;
            var loadedScenes = new List<string>();

            await UnloadScenes();

            var sceneCount = SceneManager.sceneCount;
            
            for (var i = 0; i < sceneCount; i++) {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = _activeSceneGroup.scenes.Count;
            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            for (var i = 0; i < totalScenesToLoad; i++) {
                var sceneData = group.scenes[i];

                if (!reloadDupScenes && loadedScenes.Contains(sceneData.Name)) continue;
                
                if (sceneData.reference.State == SceneReferenceState.Regular) {
                    var operation = SceneManager.LoadSceneAsync(sceneData.reference.Path, LoadSceneMode.Additive);
                    operationGroup.Operations.Add(operation);
                } 
#if UNITY_ADDRESSABLES
                else if (sceneData.Reference.State == SceneReferenceState.Addressable) {
                    var sceneHandle = Addressables.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);
                    handleGroup.Handles.Add(sceneHandle);
                }
#endif
                OnSceneLoaded.Invoke(sceneData.Name);
            }
            
            // Aguarda até que todas as operações assíncronas terminem
            while (!operationGroup.IsDone 
#if UNITY_ADDRESSABLES
                || !handleGroup.IsDone
#endif
            ) {
                progress?.Report(operationGroup.Progress 
#if UNITY_ADDRESSABLES
                    + handleGroup.Progress
#endif
                    / 2);
                await Task.Delay(100);
            }

            var activeScene = SceneManager.GetSceneByName(_activeSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid()) {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded.Invoke();
        }

        /// <summary>
        /// Descarrega todas as cenas, exceto a cena ativa.
        /// </summary>
        public async Task UnloadScenes() { 
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;
            
            var sceneCount = SceneManager.sceneCount;

            for (var i = sceneCount - 1; i > 0; i--) {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;
                
                var sceneName = sceneAt.name;
                if (sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
                
#if UNITY_ADDRESSABLES
                if (handleGroup.Handles.Any(h => h.IsValid() && h.Result.Scene.name == sceneName)) continue;
#endif
                
                scenes.Add(sceneName);
            }
            
            var operationGroup = new AsyncOperationGroup(scenes.Count);
            
            foreach (var scene in scenes) { 
                var operation = SceneManager.UnloadSceneAsync(scene);
                if (operation == null) continue;
                
                operationGroup.Operations.Add(operation);

                OnSceneUnloaded.Invoke(scene);
            }
            
#if UNITY_ADDRESSABLES
            foreach (var handle in handleGroup.Handles) {
                if (handle.IsValid()) {
                    Addressables.UnloadSceneAsync(handle);
                }
            }
            handleGroup.Handles.Clear();
#endif

            while (!operationGroup.IsDone) {
                await Task.Delay(100); // Evita loop apertado
            }

            // Libera recursos não utilizados da memória
            await Resources.UnloadUnusedAssets();
        }
    }
    
    /// <summary>
    /// Estrutura que gerencia um grupo de operações assíncronas do SceneManager.
    /// </summary>
    public readonly struct AsyncOperationGroup { 
        /// <summary> Lista de operações assíncronas. </summary>
        public readonly List<AsyncOperation> Operations;

        /// <summary> Progresso médio das operações assíncronas. </summary>
        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);

        /// <summary> Indica se todas as operações foram concluídas. </summary>
        public bool IsDone => Operations.All(o => o.isDone);

        /// <summary>
        /// Inicializa um novo grupo de operações assíncronas.
        /// </summary>
        /// <param name="initialCapacity">Capacidade inicial da lista de operações.</param>
        public AsyncOperationGroup(int initialCapacity) {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }

#if UNITY_ADDRESSABLES
    /// <summary>
    /// Estrutura que gerencia um grupo de operações assíncronas para Addressables.
    /// </summary>
    public readonly struct AsyncOperationHandleGroup {
        /// <summary> Lista de handles de carregamento de Addressables. </summary>
        public readonly List<AsyncOperationHandle<SceneInstance>> Handles;
        
        /// <summary> Progresso médio das operações assíncronas de Addressables. </summary>
        public float Progress => Handles.Count == 0 ? 0 : Handles.Average(h => h.PercentComplete);

        /// <summary> Indica se todas as operações foram concluídas. </summary>
        public bool IsDone => Handles.Count == 0 || Handles.All(o => o.IsDone);

        /// <summary>
        /// Inicializa um novo grupo de operações assíncronas para Addressables.
        /// </summary>
        /// <param name="initialCapacity">Capacidade inicial da lista de handles.</param>
        public AsyncOperationHandleGroup(int initialCapacity) {
            Handles = new List<AsyncOperationHandle<SceneInstance>>(initialCapacity);
        }
    }
#endif
}
