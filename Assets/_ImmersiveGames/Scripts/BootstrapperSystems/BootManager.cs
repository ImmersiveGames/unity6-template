using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;
using _ImmersiveGames.Scripts.SceneManagerSystems;
using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BootstrapperSystems {
    public class BootManager : MonoBehaviour
    {
        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            Debug.Log("BootManager: Inicializando sistemas...");

            // Certifica-se de que o ServiceLocator esteja carregado antes de inicializar os outros sistemas
            ServiceLocatorBootstrapper.Instance.BootstrapOnDemand();
            DebugBootstrapper.Initialize();

            // Inicializa sistemas principais
            SceneBootstrapper.Instance.Initialize();
            EventBusManager.Initialize();
            //GameplayBootstrapper.Initialize();

            Debug.Log("BootManager: Inicialização concluída.");
        }
    }
}