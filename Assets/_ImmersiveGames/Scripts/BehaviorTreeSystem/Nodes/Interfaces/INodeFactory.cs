namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public interface INodeFactory
    {
        IBehaviorNode CreateNode(BehaviorNodeType nodeType, NodeConfig config, BlackboardSo blackboard);
    }
}