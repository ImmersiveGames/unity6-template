using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BusEventSystems {
    public class TestBusBinding : MonoBehaviour {
        private int _health;
        private int _mana;
        private EventBinding<TestEvents> _testEventBinging;
        private EventBinding<PlayerEvents> _playerEventBinging;

        private void Awake() {
            _health = 100;
            _mana = 100;
        }

        private void OnEnable() {
            _testEventBinging = new EventBinding<TestEvents>(HandleTestEvents);
            EventBus<TestEvents>.Register(_testEventBinging);
            
            _playerEventBinging = new EventBinding<PlayerEvents>(HandlePlayerEvents);
            EventBus<PlayerEvents>.Register(_playerEventBinging);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.V)) {
                EventBus<TestEvents>.Raise(new TestEvents());
            }
            if (Input.GetKeyDown(KeyCode.B)) {
                EventBus<PlayerEvents>.Raise(new PlayerEvents { 
                    Health = _health, Mana = _mana
                });
            }
        }

        private void OnDisable() {
            EventBus<TestEvents>.Unregister(_testEventBinging);
            EventBus<PlayerEvents>.Unregister(_playerEventBinging);
            EventBusManager.ClearAllBuses();
        }

        private void HandleTestEvents() {
            DebugManager.Log<TestBusBinding>($"Test event received!");
        }

        private void HandlePlayerEvents(PlayerEvents events) {
            DebugManager.Log<TestBusBinding>($"Player events received: {events.Health} / {events.Mana}");
        }
    }
}