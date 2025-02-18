using _ImmersiveGames.Scripts.SpawnSystems.Interface;
using _ImmersiveGames.Scripts.SpawnSystems.Strategy;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    public abstract class EntitySpawnManager : MonoBehaviour {
        [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;
        
        protected ISpawnPointStrategy SpawnPointStrategy;

        protected enum SpawnPointStrategyType {
            Linear,
            Random
        }

        protected virtual void Awake() {
            SpawnPointStrategy = spawnPointStrategyType switch {
                SpawnPointStrategyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => SpawnPointStrategy
            };
        }

        public abstract void Spawn();
    }
}