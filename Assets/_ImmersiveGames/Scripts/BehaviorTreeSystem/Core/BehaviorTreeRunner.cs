using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Core {
    public class BehaviorTreeRunner
    {
        private readonly INode _rootNode;
        public NodeState CurrentState { get; private set; }

        public BehaviorTreeRunner(INode rootNode)
        {
            _rootNode = rootNode;
            CurrentState = NodeState.Running;
        }

        public void Tick()
        {
            if (CurrentState == NodeState.Running)
                CurrentState = _rootNode.Tick();
        }
    }
    public class BehaviorTree
    {
        private readonly INode _rootNode;

        public BehaviorTree(INode rootNode)
        {
            _rootNode = rootNode;
        }

        public void Tick()
        {
            _rootNode?.Tick();
        }
    }
}