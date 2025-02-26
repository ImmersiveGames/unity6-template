using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class Selector : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new();
        private int currentChildIndex = 0;

        public void AddChild(IBehaviorNode child) => children.Add(child);
        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            if (children.Count == 0) return NodeState.Failure;

            while (currentChildIndex < children.Count) {
                var state = children[currentChildIndex].Execute();

                switch (state) {
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        currentChildIndex = 0; // Reseta para a próxima execução
                        return NodeState.Success;
                    case NodeState.Failure:
                    default:
                        currentChildIndex++; // Tenta o próximo filho se o atual falhou
                        break;
                }
            }

            currentChildIndex = 0; // Reseta após falhar todos
            return NodeState.Failure;
        }
    }
}