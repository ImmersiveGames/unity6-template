using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    [CreateAssetMenu(fileName = "HasTargetCondition", menuName = "ImmersiveGames/Behavior/Strategies/Condition/HasTarget")]
    public class HasTargetCondition : ConditionStrategySo
    {
        public override bool Evaluate(BlackboardSo blackboard)
        {
            return blackboard.Target != null;
        }
    }
}