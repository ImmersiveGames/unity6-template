using _ImmersiveGames.Scripts.Utils.MediatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers.Testes {
    public abstract class Pickup : MonoBehaviour, IVisitor {
        protected abstract void ApplyPickupEffect(Entity entity);

        public void Visit<T>(T visitable) where T : Component, IVisitable {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (visitable is Entity entity) {
                ApplyPickupEffect(entity);
            }
        }

        public void OnTriggerEnter(Collider other) {
            other.GetComponent<IVisitable>()?.Accept(this);
            Debug.Log("Picked up " + name);
            Destroy(gameObject); //melhor mudar para pool
        }
    }
}