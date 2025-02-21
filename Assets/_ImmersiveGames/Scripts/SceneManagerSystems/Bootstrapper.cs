using _ImmersiveGames.Scripts.StatesModifiers;
using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;
using _ImmersiveGames.Scripts.StatesModifiers.Testes;
using _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems;
using _ImmersiveGames.Scripts.Utils.Singletons;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    public class Bootstrapper : PersistentSingleton<Bootstrapper> {
        // NOTE: This script is intended to be placed in your first scene included in the build settings.
        static readonly int sceneIndex = 0;
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() {
            Debug.Log("Bootstrapper...");
#if UNITY_EDITOR
            // Set the bootstrapper scene to be the play mode start scene when running in the editor
            // This will cause the bootstrapper scene to be loaded first (and only once) when entering
            // play mode from the Unity Editor, regardless of which scene is currently active.
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[sceneIndex].path);
#endif
        }

        protected override void Awake() {
            base.Awake();
            // Não sei se aqui é necessário poderia ser in Inject, mas no tutorial ele usa o locator
            // Refactor: MAs acho que caberia melhor um Inject nesse caso
            ServiceLocator.Global.Register<IStatModifierFactory>(new StatModifierFactory());
        }
    }
}