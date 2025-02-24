using _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
    {
        public class RestartDecorator : IDecoratorNode
        {
            public IBehaviorNode Child { get; set; }
            private readonly IConditionStrategy restartCondition; // Condição opcional para continuar reiniciando
            private readonly BlackboardSo blackboard;

            public RestartDecorator(IBehaviorNode child, IConditionStrategy restartCondition = null, BlackboardSo blackboard = null)
            {
                Child = child;
                this.restartCondition = restartCondition;
                this.blackboard = blackboard;
            }

            public NodeState Execute()
            {
                NodeState state = Child.Execute();

                if (state == NodeState.Running)
                    return NodeState.Running;

                // Se há uma condição e ela falhou, para com o último estado
                if (restartCondition != null && !restartCondition.Evaluate(blackboard))
                    return state;

                // Reinicia o nó para a próxima execução
                return NodeState.Running;
            }
        }
    }
}