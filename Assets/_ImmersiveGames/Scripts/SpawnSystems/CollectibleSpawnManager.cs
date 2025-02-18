using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using _ImmersiveGames.Scripts.EntitySystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    public class CollectibleSpawnManager : EntitySpawnManager {
        [SerializeField] private EntityData[] collectibleData;
        [SerializeField] private int spawnInterval = 1;

        private EntitySpawner<Collectible> _spawner;

        private CountdownTimer _spawnTimer;
        private int _counter;

        protected override void Awake() {
            base.Awake();

            _spawner = new EntitySpawner<Collectible>(
                new EntityFactory<Collectible>(collectibleData),
                SpawnPointStrategy);
            
            _spawnTimer = new CountdownTimer(spawnInterval);
            _spawnTimer.OnTimerStop += () => {
                if (_counter++ >= spawnPoints.Length) {
                    _spawnTimer.Stop();
                    return;
                }
                Spawn();
                _spawnTimer.Start();
            };
        }

        private void Start() => _spawnTimer.Start();

        public override void Spawn() => _spawner.Spawn();
    }
}