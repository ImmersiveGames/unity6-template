using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems {
    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class ServiceLocatorScene : ServiceLocatorBootstrapper {
        protected override void Bootstrap() {
            Container.ConfigureForScene();            
        }
    }
}