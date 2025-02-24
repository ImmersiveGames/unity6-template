using System.Collections.Generic;
using System.Linq;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
{
    public class RandomSequence : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new List<IBehaviorNode>();
        private List<int> executionOrder = new();
        private int currentIndex = 0;

        public void AddChild(IBehaviorNode child)
        {
            children.Add(child);
            executionOrder = null; // Reseta para ser gerado na primeira execução
        }

        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            if (children.Count == 0)
                return NodeState.Success;

            // Inicializa a ordem aleatória na primeira execução
            if (executionOrder == null || executionOrder.Count != children.Count)
            {
                executionOrder = Enumerable.Range(0, children.Count).OrderBy(x => UnityEngine.Random.value).ToList();
                currentIndex = 0;
            }

            while (currentIndex < children.Count)
            {
                NodeState state = children[executionOrder[currentIndex]].Execute();

                if (state == NodeState.Running)
                    return NodeState.Running;

                if (state == NodeState.Failure)
                {
                    currentIndex = 0; // Reseta para próxima execução completa
                    return NodeState.Failure;
                }

                currentIndex++;
            }

            currentIndex = 0; // Reseta após completar todos
            return NodeState.Success;
        }
    }
}