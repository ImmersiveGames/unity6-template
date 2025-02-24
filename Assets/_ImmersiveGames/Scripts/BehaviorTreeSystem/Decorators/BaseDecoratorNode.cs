using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Decorators {
    public abstract class BaseDecoratorNode : INode
    {
        protected readonly INode Node;

        protected BaseDecoratorNode(INode node) => Node = node;

        public virtual void OnEnter() => Node.OnEnter();
        public abstract NodeState Tick();
        public virtual void OnExit() => Node.OnExit();
    }

}