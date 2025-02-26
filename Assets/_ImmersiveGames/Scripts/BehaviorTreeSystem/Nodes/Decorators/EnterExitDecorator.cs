using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class EnterExitDecorator : IDecoratorNode
    {
        private readonly IBehaviorNode node;
        private readonly BlackboardSo blackboard;
        private readonly string enterEventKey;
        private readonly string exitEventKey;
        private bool hasEntered;

        public EnterExitDecorator(IBehaviorNode node, BlackboardSo blackboard, string enterEventKey, string exitEventKey)
        {
            this.node = node;
            this.blackboard = blackboard;
            this.enterEventKey = enterEventKey;
            this.exitEventKey = exitEventKey;
            DebugManager.LogVerbose<EnterExitDecorator>("Decorator initialized");
        }

        public IBehaviorNode Child { get => node; set => throw new NotSupportedException("Child is read-only in EnterExitDecorator"); }

        public NodeState Execute()
        {
            DebugManager.LogVerbose<EnterExitDecorator>("Executing decorator...");
            if (!hasEntered && !string.IsNullOrEmpty(enterEventKey))
            {
                DebugManager.Log<EnterExitDecorator>($"Emitting enter event: {enterEventKey}");
                EventBus<IBehaviorEvent>.Raise(new BehaviorEvent(enterEventKey, blackboard));
                hasEntered = true;
            }

            var currentState = node.Execute();

            if (hasEntered && (currentState == NodeState.Success || currentState == NodeState.Failure))
            {
                if (!string.IsNullOrEmpty(exitEventKey))
                {
                    DebugManager.Log<EnterExitDecorator>($"Emitting exit event: {exitEventKey} with state: {currentState}");
                    EventBus<IBehaviorEvent>.Raise(new BehaviorEvent(exitEventKey, blackboard, currentState));
                }
                hasEntered = false;
            }

            DebugManager.Log<EnterExitDecorator>($"Decorator returning state: {currentState}");
            return currentState;
        }
    }
}