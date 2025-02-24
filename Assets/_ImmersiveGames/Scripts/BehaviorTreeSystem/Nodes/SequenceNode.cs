using System.Collections.Generic;
using _ImmersiveGames.Scripts.Utils.DebugSystems;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
{
    public class SequenceNode : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new();

        public void AddChild(IBehaviorNode child) => children.Add(child);
        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            DebugManager.LogVerbose<SequenceNode>("Starting sequence execution...");
            foreach (var child in children)
            {
                DebugManager.Log<SequenceNode>($"Executing child: {child.GetType().Name}");
                var state = child.Execute();
                DebugManager.Log<SequenceNode>($"Child returned state: {state}");

                if (state != NodeState.Success)
                {
                    if (state == NodeState.Failure)
                        DebugManager.LogWarning<SequenceNode>("Sequence interrupted due to child failure");
                    else
                        DebugManager.LogVerbose<SequenceNode>("Sequence paused due to Running state");
                    return state;
                }
            }
            DebugManager.Log<SequenceNode>("Sequence completed successfully");
            return NodeState.Success;
        }
    }
}