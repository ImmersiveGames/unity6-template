using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    [CreateAssetMenu(fileName = "MoveToTargetStrategy", menuName = "ImmersiveGames/Behavior/Strategies/MoveToTarget")]
    public class MoveToTargetStrategySo : ActionStrategySo
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float stoppingDistance = 1f;

        public override NodeState Execute(BlackboardSo blackboard)
        {
            if (blackboard.Target == null || blackboard.Owner == null) return NodeState.Failure;

            var direction = (blackboard.Target.position - blackboard.Position).normalized;
            blackboard.Position += direction * speed * Time.deltaTime;
            blackboard.Owner.transform.position = blackboard.Position;

            return Vector3.Distance(blackboard.Position, blackboard.Target.position) <= stoppingDistance
                ? NodeState.Success
                : NodeState.Running;
        }
    }
}