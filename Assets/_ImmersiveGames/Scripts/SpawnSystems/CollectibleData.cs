using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "ImmersiveGames/Platform/Collectible Data")]
    public class CollectibleData : EntityData {
        public int score;
        // additional properties specific to collectibles
    }
}