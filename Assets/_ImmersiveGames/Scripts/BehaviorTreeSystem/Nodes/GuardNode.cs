using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class GuardNode : INode
    {
        private readonly INode _node;
        private readonly Func<bool> _condition;

        public GuardNode(INode node, Func<bool> condition)
        {
            _node = node;
            _condition = condition;
        }

        public void OnEnter() { }

        public NodeState Tick()
        {
            return _condition() ? _node.Tick() : NodeState.Failure;
        }

        public void OnExit() { }
    }
}