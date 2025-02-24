namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public class Repeat : IDecoratorNode
    {
        public IBehaviorNode Child { get; set; }
        private readonly int times;
        private int currentIteration;

        public Repeat(IBehaviorNode child, int times)
        {
            Child = child;
            this.times = times;
        }

        public NodeState Execute()
        {
            if (currentIteration >= times)
            {
                currentIteration = 0;
                return NodeState.Success;
            }

            NodeState state = Child.Execute();
            if (state == NodeState.Success) currentIteration++;
            return state == NodeState.Failure ? NodeState.Failure : NodeState.Running;
        }
    }
}