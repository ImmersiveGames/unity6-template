using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Decorators {
    public class ConditionalDecoratorNode : BaseDecoratorNode {
        private readonly Func<bool> _condition;
        private readonly Action _onConditionMet;
        private bool _isConditionMet;

        public ConditionalDecoratorNode(INode node, Func<bool> condition, Action onConditionMet = null)
            : base(node) {
            _condition = condition;
            _onConditionMet = onConditionMet ?? (() => { });
        }

        public override NodeState Tick() {
            if (_condition()) {
                _onConditionMet?.Invoke();
                _isConditionMet = true;
            }

            return _isConditionMet ? NodeState.Success : Node.Tick();
        }
    }
}