using System.Collections.Generic;
using _ImmersiveGames.Scripts.DebugSystems;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
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

                switch (state) {
                    case NodeState.Success:
                        continue;
                    case NodeState.Failure:
                        DebugManager.LogWarning<SequenceNode>("Sequence interrupted due to child failure");
                        break;
                    case NodeState.Running:
                    default:
                        DebugManager.LogVerbose<SequenceNode>("Sequence paused due to Running state");
                        break;
                }

                return state;
            }
            DebugManager.Log<SequenceNode>("Sequence completed successfully");
            return NodeState.Success;
        }
    }
}