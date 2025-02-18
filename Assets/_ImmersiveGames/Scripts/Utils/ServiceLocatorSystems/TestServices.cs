using System.Collections.Generic;
using _ImmersiveGames.Scripts.Utils.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems {
    public class TestServices : MonoBehaviour {
        private ILocalization _localization;
        private ISerializer _serializer;
        private IAudioService _audioService;
        private IGameService _gameService;
        
        //Aqui ele considera fazer um instalador de serviços, preciso pesquisar sobre isso
        private readonly List<Object> _services;
        public TestServices(List<Object> services) {
            _services = services;
        }

        private void Awake() {
            ServiceLocator.Global.Register(_localization = new MockLocalization());
            ServiceLocator.ForSceneOf(this).Register(_gameService = new MockGameService());
            ServiceLocator.For(this).Register(_serializer = new MockSerializer());
            
            var serviceLocator = ServiceLocator.For(this);
            foreach (var service in _services) {
                serviceLocator.Register(service.GetType(), service);
            }
            
        }

        private void Start() {
            DebugManager.Log<TestServices>("**** Start Localization Test ****");
            ServiceLocator.For(this)
                .Get(out _serializer)
                .Get(out _localization)
                .Get(out _gameService)
                .Get(out _audioService);
            DebugManager.Log<TestServices>(_localization.GetLocalizedString("sapo"));
            _serializer.Serialize(_localization);
            _audioService.PlaySound("testSound");
            _gameService.Initialize();
        }
    }
}