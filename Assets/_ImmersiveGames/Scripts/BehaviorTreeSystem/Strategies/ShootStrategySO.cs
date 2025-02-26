using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "ImmersiveGames/Behavior/Strategies/Shoot")]
    public class ShootStrategySo :ActionStrategySo
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float cooldown = 1f;
        private float lastShot;

        public override NodeState Execute(BlackboardSo blackboard)
        {
            if (Time.time < lastShot + cooldown || blackboard.Owner == null || projectilePrefab == null)
                return NodeState.Running;

            var spawnPoint = blackboard.SpawnPoint ?? blackboard.Owner.transform;
            var direction = blackboard.Target != null
                ? (blackboard.Target.position - spawnPoint.position).normalized
                : spawnPoint.forward;

            var projectile = Object.Instantiate(projectilePrefab, spawnPoint.position, Quaternion.LookRotation(direction));
            if (projectile.TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = direction * speed;

            lastShot = Time.time;
            return NodeState.Success;
        }
    }
}