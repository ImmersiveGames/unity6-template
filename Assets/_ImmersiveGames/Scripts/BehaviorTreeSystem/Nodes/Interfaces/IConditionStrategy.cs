namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public interface IConditionStrategy
    {
        bool Evaluate(BlackboardSo blackboard);
    }
}