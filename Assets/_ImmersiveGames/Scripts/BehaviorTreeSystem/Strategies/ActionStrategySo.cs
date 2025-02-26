using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public abstract class ActionStrategySo : ScriptableObject, IActionStrategy {
        public abstract NodeState Execute(BlackboardSo blackboard);
    }
}