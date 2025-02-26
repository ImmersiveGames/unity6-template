using _ImmersiveGames.Scripts.EntitySystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    public interface IEntityFactory<out T> where T : Entity {
        T Create(Transform spawnPoint);
    }
}