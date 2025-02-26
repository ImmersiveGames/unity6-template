
using _ImmersiveGames.Scripts.ChannelEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems {
    public class Health : MonoBehaviour {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;

        private int _currentHealth;
        
        public bool IsDead => _currentHealth <= 0;

        private void Awake() {
            _currentHealth = maxHealth;
        }

        private void Start() {
            PublishHealthPercentage();
        }
        
        public void TakeDamage(int damage) {
            _currentHealth -= damage;
            PublishHealthPercentage();
        }

        private void PublishHealthPercentage() {
            DebugManager.Log<Health>("Publish Channel");
            if (playerHealthChannel != null)
                playerHealthChannel.Invoke(_currentHealth / (float) maxHealth);
        }
    }
}