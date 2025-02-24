using _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
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

            NodeState state = Child.Execute();

            if (state == NodeState.Running)
                return NodeState.Running;

            // Se o filho terminou (Success ou Failure), reinicia
            if (state == NodeState.Success || state == NodeState.Failure)
                return NodeState.Running; // Continua em Running enquanto a condição for verdadeira

            return state; // Nunca deve chegar aqui, mas mantém segurança
        }
    }
}