using System;
using System.Collections.Generic;
using System.Linq;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public class RandomPersistentSelector : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new();
        private List<int> remainingIndices = new();
        private int currentIndex = -1;
        private bool anySuccess;

        public void AddChild(IBehaviorNode child)
        {
            children.Add(child);
            remainingIndices = null; // Reseta para ser gerado na primeira execução
        }

        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            if (children.Count == 0)
                return NodeState.Failure;

            // Inicializa os índices restantes na primeira execução
            if (remainingIndices == null || remainingIndices.Count == 0)
            {
                remainingIndices = Enumerable.Range(0, children.Count).ToList();
                currentIndex = -1;
                anySuccess = false;
            }

            // Se todos os filhos foram testados
            if (remainingIndices.Count == 0)
            {
                currentIndex = -1; // Reseta para próxima execução completa
                return anySuccess ? NodeState.Success : NodeState.Failure;
            }

            // Escolhe um novo filho aleatório se não estiver executando um
            if (currentIndex == -1)
            {
                var randomChoice = UnityEngine.Random.Range(0, remainingIndices.Count);
                currentIndex = remainingIndices[randomChoice];
            }

            var state = children[currentIndex].Execute();

            switch (state) {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    anySuccess = true;
                    break;
                case NodeState.Failure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Remove o filho atual, independentemente de sucesso ou falha
            remainingIndices.Remove(currentIndex);
            currentIndex = -1;

            // Continua até testar todos ou retorna Running enquanto há filhos
            return remainingIndices.Count > 0 ? NodeState.Running : (anySuccess ? NodeState.Success : NodeState.Failure);
        }
    }
}