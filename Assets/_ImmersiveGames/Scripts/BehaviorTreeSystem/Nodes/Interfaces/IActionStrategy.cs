namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public enum NodeState { Running, Success, Failure }

    // Interface para estratégias de ação
    public interface IActionStrategy
    {
        NodeState Execute(BlackboardSo blackboard);
    }

    // Interface para estratégias de condição
}