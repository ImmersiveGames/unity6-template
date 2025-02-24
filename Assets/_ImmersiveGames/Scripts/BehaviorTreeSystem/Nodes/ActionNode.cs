using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class ActionNode : INode
    {
        private readonly Func<NodeState> _action;

        public ActionNode(Func<NodeState> action) => _action = action;

        public void OnEnter() { }

        public NodeState Tick() => _action.Invoke();

        public void OnExit() { }
    }
}