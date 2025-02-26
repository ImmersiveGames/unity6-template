using _ImmersiveGames.Scripts.Utils.Extensions;
using _ImmersiveGames.Scripts.Utils.Singletons;
using UnityEngine;

namespace _ImmersiveGames.Scripts.ServiceLocatorSystems
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ServiceLocator))]
    public class ServiceLocatorBootstrapper : PersistentSingleton<ServiceLocatorBootstrapper>
    {
        private ServiceLocator _container;
        internal ServiceLocator Container => _container.OrNull() ?? (_container = GetComponent<ServiceLocator>());

        private bool _hasBeenBootstrapped;

        public void BootstrapOnDemand()
        {
            if (_hasBeenBootstrapped) return;
            _hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected virtual void Bootstrap()
        {
            Debug.Log("ServiceLocatorBootstrapper inicializado.");
        }
    }
}