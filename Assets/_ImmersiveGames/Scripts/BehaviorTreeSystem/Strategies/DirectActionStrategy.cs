using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;
using _ImmersiveGames.Scripts.Utils.GoapSystem;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    public class DirectActionStrategy : IActionStrategy
    {
        private readonly Action _action;
        private IActionStrategy _actionStrategyImplementation;

        public DirectActionStrategy(Action action) => _action = action;

        public NodeState ExecuteAction()
        {
            _action();
            return NodeState.Success;
        }

        public bool CanPerform => _actionStrategyImplementation.CanPerform;

        public bool Complete => _actionStrategyImplementation.Complete;
    }

}