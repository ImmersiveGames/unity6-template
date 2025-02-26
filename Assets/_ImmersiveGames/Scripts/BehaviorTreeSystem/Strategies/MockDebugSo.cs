using _ImmersiveGames.Scripts.Utils.Extensions;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    [CreateAssetMenu(fileName = "MockStrategy", menuName = "ImmersiveGames/Behavior/Strategies/Debug")]
    public class MockDebugSo : ActionStrategySo {
        [SerializeField] public string text;
        public override NodeState Execute(BlackboardSo blackboard) {
            Debug.Log(text);
            return text.IsNullOrEmpty() ? NodeState.Failure : NodeState.Success;
        }
    }
}