using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    public interface ISpawnPointStrategy {
        Transform NextSpawnPoint();
    }
}