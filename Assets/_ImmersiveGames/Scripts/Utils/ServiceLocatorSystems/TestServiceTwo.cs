using _ImmersiveGames.Scripts.Utils.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems {
    public class TestServiceTwo:MonoBehaviour
    {
        private IAudioService _audioService;
        private IGameService _gameService;

        private void Awake() {
            ServiceLocator.Global.Register(_audioService = new MockAudioService());
            ServiceLocator.ForSceneOf(this).Register(_gameService = new MockMapService());
        }

        private void Start() {
            DebugManager.Log<TestServiceTwo>("**** Start Other Scene Test ****");
            ServiceLocator.For(this)
                .Get(out _gameService)
                .Get(out _audioService);
            _audioService.PlaySound("testSound 22");
            _gameService.Initialize();
        }
    }
}