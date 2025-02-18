using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems.Interface {
    public interface ISpawnPointStrategy {
        Transform NextSpawnPoint();
    }
}