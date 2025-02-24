using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    [CreateAssetMenu(fileName = "DistanceToTargetCondition", menuName = "ImmersiveGames/Behavior/Strategies/Condition/DistanceToTarget")]
    public class DistanceToTargetCondition : ConditionStrategySo
    {
        [SerializeField] private float maxDistance = 5f;

        public override bool Evaluate(BlackboardSo blackboard)
        {
            return blackboard.Target != null && Vector3.Distance(blackboard.Position, blackboard.Target.position) < maxDistance;
        }
    }
}