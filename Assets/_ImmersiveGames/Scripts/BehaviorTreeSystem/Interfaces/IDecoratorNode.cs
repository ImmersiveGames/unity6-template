namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces {
    public interface IDecoratorNode : INode
    {
        INode GetDecoratedNode();
    }
}