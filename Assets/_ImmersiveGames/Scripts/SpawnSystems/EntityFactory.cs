using _ImmersiveGames.Scripts.EntitySystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.SpawnSystems {
    public class EntityFactory<T> : IEntityFactory<T> where T : Entity {
        private readonly EntityData[] _data;
        
        public EntityFactory(EntityData[] data) {
            _data = data;
        }
        
        public T Create(Transform spawnPoint) {
            var entityData = _data[Random.Range(0, _data.Length)];
            var instance = Object.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
            return instance.GetComponent<T>();
        }
    }
}