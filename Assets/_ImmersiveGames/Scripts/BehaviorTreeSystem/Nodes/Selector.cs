using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
{
    public class Selector : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new List<IBehaviorNode>();
        private int currentChildIndex = 0;

        public void AddChild(IBehaviorNode child) => children.Add(child);
        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            if (children.Count == 0) return NodeState.Failure;

            while (currentChildIndex < children.Count)
            {
                NodeState state = children[currentChildIndex].Execute();

                if (state == NodeState.Running)
                    return NodeState.Running;

                if (state == NodeState.Success)
                {
                    currentChildIndex = 0; // Reseta para a próxima execução
                    return NodeState.Success;
                }

                currentChildIndex++; // Tenta o próximo filho se o atual falhou
            }

            currentChildIndex = 0; // Reseta após falhar todos
            return NodeState.Failure;
        }
    }
}