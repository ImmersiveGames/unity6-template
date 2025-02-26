using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public abstract class ConditionStrategySo : ScriptableObject, IConditionStrategy
    {
        public abstract bool Evaluate(BlackboardSo blackboard);
    }
}