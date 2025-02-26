using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public interface INodeFactory
    {
        IBehaviorNode CreateNode(BehaviorNodeType nodeType, NodeConfig config, BlackboardSo blackboard);
    }
}