using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies {
    public class BurstActionStrategy<T> : IActionStrategy<T>
    {
        private int _actionsExecuted = 0;
        private const int BurstCount = 3;
        private float _lastActionTime;

        private readonly Action<T> _action;

        public BurstActionStrategy(Action<T> action)
        {
            _action = action;
        }

        public NodeState ExecuteAction(T agent)
        {
            if (_actionsExecuted >= BurstCount)
            {
                _actionsExecuted = 0;
                return NodeState.Success;
            }

            if (Time.realtimeSinceStartup < _lastActionTime + 0.2f)
                return NodeState.Running;

            _action(agent);
            _actionsExecuted++;
            _lastActionTime = Time.realtimeSinceStartup;

            return NodeState.Running;
        }
    }
}