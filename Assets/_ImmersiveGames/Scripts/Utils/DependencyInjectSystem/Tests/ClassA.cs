using System;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests {
    public class ClassA : MonoBehaviour {
        private ServiceA _serviceA;

        [Inject]
        public void Init(ServiceA serviceA) {
            _serviceA = serviceA;
        }
        
        [Inject] private IEnvironmentSystem _environmentSystem;

        private void Start() {
            _serviceA.Initialize($"Service A Initialized from Class A");
            _environmentSystem.Initialize();
        }
    }
}