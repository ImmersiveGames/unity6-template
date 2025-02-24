using System.Collections.Generic;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
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
            bool isAnyRunning = false;
            int successCount = 0;
            int failureCount = 0;

            foreach (var child in children)
            {
                var result = child.Execute();
                if (result == NodeState.Running)
                {
                    isAnyRunning = true;
                }
                else if (result == NodeState.Success)
                {
                    successCount++;
                    if (interruptOnSuccess)
                        return NodeState.Success; // Interrompe imediatamente se configurado
                }
                else if (result == NodeState.Failure)
                {
                    failureCount++;
                    if (requireAllSuccess)
                        return NodeState.Failure; // Falha imediatamente se todos devem ter sucesso
                }
            }

            // Se algum filho ainda está rodando, retorna Running
            if (isAnyRunning)
                return NodeState.Running;

            // Avaliação final após todos os filhos terminarem
            if (requireAllSuccess)
            {
                if (successCount == children.Count)
                    return NodeState.Success;
                return NodeState.Failure;
            }
            else
            {
                if (successCount > 0)
                    return NodeState.Success;
                return NodeState.Failure;
            }
        }
    }
}