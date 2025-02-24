using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class DelayDecorator : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly float delay;
        private float elapsedTime;

        public DelayDecorator(IBehaviorNode child, float delay)
        {
            Child = child;
            this.delay = delay;
        }

        public NodeState Execute()
        {
            elapsedTime += Time.deltaTime;
            return elapsedTime < delay ? NodeState.Running : Child.Execute();
        }
    }
}