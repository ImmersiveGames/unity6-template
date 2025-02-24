using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    [CreateAssetMenu(fileName = "CustomValueCondition", menuName = "ImmersiveGames/Behavior/Strategies/Condition/CustomValue")]
    public class CustomValueCondition : ConditionStrategySo
    {
        [SerializeField] private string key = "CustomCondition";
        [SerializeField] private bool expectedValue = true;

        public override bool Evaluate(BlackboardSo blackboard)
        {
            return blackboard.GetValue(key, !expectedValue) == expectedValue;
        }
    }
}