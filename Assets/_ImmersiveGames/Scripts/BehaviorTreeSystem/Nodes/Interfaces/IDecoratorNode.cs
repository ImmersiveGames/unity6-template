namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public interface IDecoratorNode : IBehaviorNode
    {
        IBehaviorNode Child { get; set; }
    }
}