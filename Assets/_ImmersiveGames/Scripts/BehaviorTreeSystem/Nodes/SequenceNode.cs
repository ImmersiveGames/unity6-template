using System.Collections.Generic;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class SequenceNode : CompositeNode
    {
        private int _currentNodeIndex = 0;

        public SequenceNode(List<INode> nodes) : base(nodes) { }

        public override void OnEnter() => _currentNodeIndex = 0;

        public override NodeState Tick()
        {
            while (_currentNodeIndex < Nodes.Count)
            {
                NodeState state = Nodes[_currentNodeIndex].Tick();
                if (state != NodeState.Success)
                    return state;
                _currentNodeIndex++;
            }
            return NodeState.Success;
        }

        public override void OnExit() => _currentNodeIndex = 0;
    }
}