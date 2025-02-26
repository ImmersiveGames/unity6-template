namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public class GenericActionNode : IBehaviorNode
    {
        private readonly IActionStrategy strategy;
        public readonly BlackboardSo Blackboard;

        public GenericActionNode(BlackboardSo blackboard, IActionStrategy strategy)
        {
            Blackboard = blackboard;
            this.strategy = strategy;
        }

        public NodeState Execute()
        {
            return strategy.Execute(Blackboard);
        }
    }
}