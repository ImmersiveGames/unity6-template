using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public interface ICompositeNode : IBehaviorNode
    {
        void AddChild(IBehaviorNode child);
        IReadOnlyList<IBehaviorNode> GetChildren();
    }
}