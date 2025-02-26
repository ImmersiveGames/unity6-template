using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [CreateAssetMenu(fileName = "CompositeCondition", menuName = "ImmersiveGames/Behavior/Strategies/Condition/Composite")]
    public class CompositeConditionSo : ConditionStrategySo
    {
        private enum LogicOperator { And, Or }
        [SerializeField] private LogicOperator logic = LogicOperator.And;
        [SerializeField] private List<ConditionStrategySo> conditions = new();

        public override bool Evaluate(BlackboardSo blackboard)
        {
            DebugManager.LogVerbose<CompositeConditionSo>("Evaluating composite condition...");
            if (conditions == null || conditions.Count == 0)
            {
                DebugManager.LogWarning<CompositeConditionSo>("No conditions defined, returning false");
                return false;
            }

            bool result;
            if (logic == LogicOperator.And)
            {
                result = conditions.All(c =>
                {
                    var eval = c.Evaluate(blackboard);
                    DebugManager.LogVerbose<CompositeConditionSo>($"Condition {c.name} evaluated to {eval}");
                    return eval;
                });
            }
            else
            {
                result = conditions.Any(c =>
                {
                    var eval = c.Evaluate(blackboard);
                    DebugManager.LogVerbose<CompositeConditionSo>($"Condition {c.name} evaluated to {eval}");
                    return eval;
                });
            }

            DebugManager.Log<CompositeConditionSo>($"Composite result ({logic}): {result}");
            return result;
        }
    }
}