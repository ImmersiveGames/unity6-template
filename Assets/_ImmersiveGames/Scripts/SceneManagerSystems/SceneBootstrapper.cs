using _ImmersiveGames.Scripts.Utils.Singletons;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    public class SceneBootstrapper : PersistentSingleton<SceneBootstrapper>
    {
        private bool _isInitialized = false;
        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            Debug.Log("SceneBootstrapper inicializado.");
            // Lógica de gerenciamento de cenas
        }
    }
}