using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public interface ICompositeNode : IBehaviorNode
    {
        void AddChild(IBehaviorNode child);
        IReadOnlyList<IBehaviorNode> GetChildren();
    }
}