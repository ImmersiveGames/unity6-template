using System.Collections.Generic;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes {
    public abstract class CompositeNode : INode
    {
        protected readonly List<INode> Nodes;

        protected CompositeNode(List<INode> nodes) => Nodes = nodes ?? new List<INode>();

        public abstract void OnEnter();
        public abstract NodeState Tick();
        public abstract void OnExit();
    }
}