using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [CreateAssetMenu(fileName = "CompareValueCondition", menuName = "ImmersiveGames/Behavior/Strategies/Condition/CompareValue")]
    public class CompareValueCondition : ConditionStrategySo
    {
        private enum ComparisonType { Equals, GreaterThan, LessThan }
        [SerializeField] private string key;
        [SerializeField] private float value;
        [SerializeField] private ComparisonType comparison;

        public override bool Evaluate(BlackboardSo blackboard)
        {
            DebugManager.LogVerbose<CompareValueCondition>($"Evaluating {comparison} for key '{key}' with value {value}");
            if (string.IsNullOrEmpty(key))
            {
                DebugManager.LogError<CompareValueCondition>("Key is empty, returning false");
                return false;
            }

            var blackboardValue = blackboard.GetValue(key, 0f);
            var result = comparison switch
            {
                ComparisonType.Equals => Mathf.Approximately(blackboardValue, value),
                ComparisonType.GreaterThan => blackboardValue > value,
                ComparisonType.LessThan => blackboardValue < value,
                _ => false
            };

            DebugManager.Log<CompareValueCondition>($"Blackboard value: {blackboardValue}, Result: {result}");
            return result;
        }
    }
}