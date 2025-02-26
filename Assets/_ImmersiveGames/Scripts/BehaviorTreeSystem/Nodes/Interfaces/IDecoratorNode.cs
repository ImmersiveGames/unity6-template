namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public interface IDecoratorNode : IBehaviorNode
    {
        IBehaviorNode Child { get; set; }
    }
}