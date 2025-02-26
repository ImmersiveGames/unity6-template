namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public interface IConditionStrategy
    {
        bool Evaluate(BlackboardSo blackboard);
    }
}