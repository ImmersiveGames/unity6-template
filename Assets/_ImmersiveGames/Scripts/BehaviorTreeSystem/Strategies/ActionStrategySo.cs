using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    public abstract class ActionStrategySo : ScriptableObject, IActionStrategy {
        public abstract NodeState Execute(BlackboardSo blackboard);
    }
}