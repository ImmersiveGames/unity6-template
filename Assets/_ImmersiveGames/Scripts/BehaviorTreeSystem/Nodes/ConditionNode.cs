using _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class ConditionNode : IBehaviorNode
    {
        private readonly IConditionStrategy condition;
        private readonly IBehaviorNode successNode;
        private readonly IBehaviorNode failureNode;
        private readonly BlackboardSo blackboard; // Adicionado

        public ConditionNode(IConditionStrategy condition, IBehaviorNode successNode, IBehaviorNode failureNode = null, BlackboardSo blackboard = null)
        {
            this.condition = condition;
            this.successNode = successNode;
            this.failureNode = failureNode;
            this.blackboard = blackboard; // Armazena o blackboard
        }

        public NodeState Execute()
        {
            // Usa o blackboard para avaliar a condição
            return condition.Evaluate(blackboard) ? successNode?.Execute() ?? NodeState.Success : failureNode?.Execute() ?? NodeState.Failure;
        }
    }
}