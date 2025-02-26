using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class RandomSelector : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new();
        private readonly int maxTimes;
        private int currentChildIndex = -1; // -1 significa que ainda não escolheu um filho
        private int attempts;

        public RandomSelector(int maxTimes)
        {
            this.maxTimes = maxTimes;
        }

        public void AddChild(IBehaviorNode child) => children.Add(child);
        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            if (children.Count == 0) return NodeState.Failure;

            // Escolhe um novo filho aleatório apenas se não estiver executando um
            if (currentChildIndex == -1 && attempts < maxTimes)
            {
                currentChildIndex = UnityEngine.Random.Range(0, children.Count);
                attempts++;
            }

            // Se todas as tentativas foram usadas, falha
            if (attempts >= maxTimes)
            {
                currentChildIndex = -1; // Reseta para próxima execução completa
                attempts = 0;
                return NodeState.Failure;
            }

            var state = children[currentChildIndex].Execute();

            switch (state) {
                case NodeState.Running:
                    return NodeState.Running;
                case NodeState.Success:
                    currentChildIndex = -1; // Reseta para próxima execução completa
                    attempts = 0;
                    return NodeState.Success;
                case NodeState.Failure:
                default:
                    // Se falhou, escolhe outro na próxima execução
                    currentChildIndex = -1;
                    return NodeState.Running; // Continua tentando até maxTimes
            }
        }
    }
}