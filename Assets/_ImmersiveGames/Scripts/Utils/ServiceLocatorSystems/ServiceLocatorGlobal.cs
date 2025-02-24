using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems {
    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class ServiceLocatorGlobal : ServiceLocatorBootstrapper {
        [SerializeField] private bool dontDestroyOnLoad = true;
        
        protected override void Bootstrap() {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }
}