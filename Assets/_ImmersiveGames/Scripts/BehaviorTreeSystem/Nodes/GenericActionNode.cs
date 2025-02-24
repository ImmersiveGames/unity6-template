using _ImmersiveGames.Scripts.Utils.GoapSystem;
using IActionStrategy = _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes.IActionStrategy;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class GenericActionNode : IBehaviorNode
    {
        private readonly IActionStrategy strategy;
        public readonly BlackboardSo blackboard;

        public GenericActionNode(BlackboardSo blackboard, IActionStrategy strategy)
        {
            this.blackboard = blackboard;
            this.strategy = strategy;
        }

        public NodeState Execute()
        {
            return strategy.Execute(blackboard);
        }
    }
}