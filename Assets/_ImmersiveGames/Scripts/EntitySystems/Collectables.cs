using _ImmersiveGames.Scripts.Utils.ChannelEventSystems.Channels;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems {
    public class Collectible : Entity {
        [SerializeField] private int score = 10; // FIXME set using Factory
        [SerializeField] private IntEventChannel scoreChannel;

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            scoreChannel.Invoke(score);
            Destroy(gameObject);
        }
    }
}