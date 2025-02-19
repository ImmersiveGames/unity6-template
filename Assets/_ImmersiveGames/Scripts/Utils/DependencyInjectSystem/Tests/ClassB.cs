using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests {
    public class ClassB : MonoBehaviour {
        [Inject]
        private ServiceA _serviceA;
        [Inject]
        private ServiceB _serviceB;
        
        private FactoryA _factoryA;

        [Inject]
        public void Init(FactoryA factoryA) {
            _factoryA = factoryA;
        }

        private void Start() {
            _serviceA.Initialize("Mensagem da Classe A Inicializando na Classe B");
            _serviceB.Initialize("Mensagem da Classe B Inicializando na Classe B");
            
            _factoryA.CreateServiceA().Initialize("ServiceA Inicializando na Factory A");
        }
    }
}