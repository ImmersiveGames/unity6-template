using System.Collections.Generic;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Extensions
{
    /// <summary>
    /// Classe de extensões para manipulação de colisores no Unity.
    /// </summary>
    public static class ColliderExtensions
    {
        /// <summary>
        /// Verifica se um colisor está colidindo com outro.
        /// </summary>
        public static bool IsCollidingWith(this Collider collider, Collider otherCollider) =>
            collider.bounds.Intersects(otherCollider.bounds);

        /// <summary>
        /// Desabilita a colisão entre dois colisores.
        /// </summary>
        public static Collider DisableCollisionWith(this Collider collider, Collider otherCollider)
        {
            Physics.IgnoreCollision(collider, otherCollider, true);
            return collider;
        }

        /// <summary>
        /// Habilita a colisão entre dois colisores.
        /// </summary>
        public static Collider EnableCollisionWith(this Collider collider, Collider otherCollider)
        {
            Physics.IgnoreCollision(collider, otherCollider, false);
            return collider;
        }

        /// <summary>
        /// Desabilita a colisão entre um colisor e uma lista de colisores.
        /// </summary>
        public static Collider DisableCollisionWith(
            this Collider collider,
            List<Collider> otherColliders
        )
        {
            if (collider == null || otherColliders == null)
                return null;
            for (var i = 0; i < otherColliders.Count; i++)
            {
                collider.DisableCollisionWith(otherColliders[i]);
            }
            return collider;
        }

        /// <summary>
        /// Habilita a colisão entre um colisor e uma lista de colisores.
        /// </summary>
        public static Collider EnableCollisionWith(
            this Collider collider,
            List<Collider> otherColliders
        )
        {
            if (collider == null || otherColliders == null)
                return null;
            for (var i = 0; i < otherColliders.Count; i++)
            {
                collider.EnableCollisionWith(otherColliders[i]);
            }
            return collider;
        }

        /// <summary>
        /// Verifica se um colisor está colidindo com algum objeto em uma camada específica usando OverlapBoxNonAlloc.
        /// </summary>
        public static bool IsCollidingWithLayer(this Collider collider, LayerMask layerMask, int size = 10)
        {
            if (collider == null)
                return false;
            
            var results = new Collider[size]; // Ajuste o tamanho conforme necessário
            var count = Physics.OverlapBoxNonAlloc(
                collider.bounds.center,
                collider.bounds.extents,
                results,
                collider.transform.rotation,
                layerMask
            );
            
            return count > 0;
        }
    }
}
