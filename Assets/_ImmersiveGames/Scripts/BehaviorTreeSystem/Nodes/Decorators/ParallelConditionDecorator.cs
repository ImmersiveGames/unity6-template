using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class ParallelConditionDecorator : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly IConditionStrategy condition; // Mudado de Func pra IConditionStrategy
        private readonly IBehaviorNode parallelAction;
        private readonly BlackboardSo blackboard; // Adicionado pra passar pro Evaluate

        public ParallelConditionDecorator(IBehaviorNode child, IConditionStrategy condition, IBehaviorNode parallelAction, BlackboardSo blackboard)
        {
            Child = child;
            this.condition = condition;
            this.parallelAction = parallelAction;
            this.blackboard = blackboard;
        }

        public NodeState Execute()
        {
            var childState = Child.Execute();

            if (condition != null && condition.Evaluate(blackboard))
            {
                var parallelState = parallelAction?.Execute() ?? NodeState.Success;
                if (parallelState != NodeState.Running) return parallelState;
            }

            return childState;
        }
    }
}