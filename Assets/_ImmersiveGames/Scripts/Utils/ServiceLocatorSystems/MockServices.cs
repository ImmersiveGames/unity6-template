using System.Collections.Generic;
using _ImmersiveGames.Scripts.Utils.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems {

    //Vamos construir sistemas mock para testes
    
    //primeiro sistema seria o sistema de localização e tradução
    public interface ILocalization {
        string GetLocalizedString(string key);
    }

    public class MockLocalization : ILocalization {
        private readonly List<string> _localizedStrings = new List<string>() { "gato", "sapo", "cão", "pássaro"};
        private readonly System.Random _random = new System.Random();
        public string GetLocalizedString(string key) {
            return _localizedStrings[_random.Next(_localizedStrings.Count)];
        }
    }

    public interface ISerializer {
        void Serialize(object obj);
    }

    public class MockSerializer : ISerializer {
        public void Serialize(object obj) {
            DebugManager.Log<MockLocalization>("MockSerializer.Serialize");
        }
    }

    public interface IAudioService {
        void PlaySound(string name);
    }

    public class MockAudioService : IAudioService {
        public void PlaySound(string name) {
            DebugManager.Log<MockLocalization>("MockAudioService.PlaySound");
        }
    }

    public interface IGameService {
        void Initialize();
    }

    public class MockGameService : IGameService {
        public void Initialize() {
            DebugManager.Log<MockLocalization>("MockGameService.Initialize");
        }
    }
    public class MockMapService : IGameService {
        public void Initialize() {
            DebugManager.Log<MockLocalization>("Mapa Inicializado");
        }
    }
}