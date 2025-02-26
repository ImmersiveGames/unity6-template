using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class AnimationDecorator : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly string trigger;
        private readonly float duration;
        private float elapsedTime;
        private bool isRunning;

        public AnimationDecorator(IBehaviorNode child, string trigger, float duration = -1f)
        {
            Child = child;
            this.trigger = trigger;
            this.duration = duration;
        }

        public NodeState Execute()
        {
            var blackboard = (Child as GenericActionNode)?.Blackboard;
            if (blackboard?.Animator == null) return Child.Execute();

            if (!isRunning)
            {
                isRunning = true;
                elapsedTime = 0;
                blackboard.Animator.SetTrigger(trigger);
               // if (duration < 0) this.duration = AnimationDuration.GetAnimationDuration(blackboard.Animator, trigger);
            }

            elapsedTime += Time.deltaTime;
            if (!(elapsedTime >= duration)) return NodeState.Running;
            isRunning = false;
            return Child.Execute();

        }
    }
}