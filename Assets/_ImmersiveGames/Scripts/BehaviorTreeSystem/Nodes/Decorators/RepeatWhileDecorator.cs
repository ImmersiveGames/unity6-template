using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public class RepeatWhileDecorator : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly IConditionStrategy condition;
        private readonly BlackboardSo blackboard;

        public RepeatWhileDecorator(IBehaviorNode child, IConditionStrategy condition, BlackboardSo blackboard)
        {
            Child = child;
            this.condition = condition;
            this.blackboard = blackboard;
        }

        public NodeState Execute()
        {
            if (!condition.Evaluate(blackboard))
                return NodeState.Success; // Para quando a condição é falsa

            var state = Child.Execute();

            return state switch {
                NodeState.Running or NodeState.Success or NodeState.Failure => NodeState.Running,
                _ => state
            };
        }
    }
}