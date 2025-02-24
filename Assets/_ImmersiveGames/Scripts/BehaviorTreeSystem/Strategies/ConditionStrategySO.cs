using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    public abstract class ConditionStrategySo : ScriptableObject, IConditionStrategy
    {
        public abstract bool Evaluate(BlackboardSo blackboard);
    }
    
    namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies
    { }
}