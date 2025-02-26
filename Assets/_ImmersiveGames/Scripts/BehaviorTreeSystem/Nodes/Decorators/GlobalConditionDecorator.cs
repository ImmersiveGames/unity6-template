using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class GlobalConditionDecorator : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly IConditionStrategy condition; // Mudado de Func pra IConditionStrategy
        private readonly IBehaviorNode overrideNode;
        private readonly BlackboardSo blackboard; // Adicionado pra passar pro Evaluate

        public GlobalConditionDecorator(IBehaviorNode child, IConditionStrategy condition, IBehaviorNode overrideNode, BlackboardSo blackboard)
        {
            Child = child;
            this.condition = condition;
            this.overrideNode = overrideNode;
            this.blackboard = blackboard;
        }

        public NodeState Execute()
        {
            // Usa o blackboard armazenado pra avaliar a condição
            return condition != null && condition.Evaluate(blackboard) 
                ? overrideNode?.Execute() ?? NodeState.Success 
                : Child.Execute();
        }
    }
}