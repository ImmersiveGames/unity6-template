using System;
using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class Parallel : ICompositeNode
    {
        private readonly List<IBehaviorNode> children = new List<IBehaviorNode>();
        private readonly bool requireAllSuccess;
        private readonly bool interruptOnSuccess;

        public Parallel(bool requireAllSuccess = true, bool interruptOnSuccess = false)
        {
            this.requireAllSuccess = requireAllSuccess;
            this.interruptOnSuccess = interruptOnSuccess;
        }

        public void AddChild(IBehaviorNode child) => children.Add(child);
        public IReadOnlyList<IBehaviorNode> GetChildren() => children.AsReadOnly();

        public NodeState Execute()
        {
            var isAnyRunning = false;
            var successCount = 0;

            foreach (var child in children) {
                var result = child.Execute();
                switch (result) {
                    case NodeState.Running:
                        isAnyRunning = true;
                        break;
                    case NodeState.Success: {
                        successCount++;
                        if (interruptOnSuccess)
                            return NodeState.Success; // Interrompe imediatamente se configurado
                        break;
                    }
                    case NodeState.Failure when requireAllSuccess:
                        return NodeState.Failure; // Falha imediatamente se todos devem ter sucesso
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Se algum filho continua rodando, retorna Running
            if (isAnyRunning)
                return NodeState.Running;

            // Avaliação final após todos os filhos terminarem
            if (requireAllSuccess)
            {
                if (successCount == children.Count)
                    return NodeState.Success;
            }
            else
            {
                if (successCount > 0)
                    return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}