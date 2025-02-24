using _ImmersiveGames.Scripts.BehaviorTreeSystem.Events;
using _ImmersiveGames.Scripts.Utils.BusEventSystems;
using _ImmersiveGames.Scripts.Utils.DebugSystems;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes
{
    public class EventReactiveNode : IBehaviorNode
    {
        private readonly IBehaviorNode successNode;
        private readonly IBehaviorNode failureNode;
        private IBehaviorEvent lastEvent;

        public EventReactiveNode(string eventKey, IBehaviorNode successNode, IBehaviorNode failureNode = null)
        {
            this.successNode = successNode;
            this.failureNode = failureNode;
            EventBus<IBehaviorEvent>.Register(new EventBinding<IBehaviorEvent>(e =>
            {
                if (e.EventKey == eventKey)
                {
                    lastEvent = e;
                    DebugManager.Log<EventReactiveNode>($"Received matching event: {eventKey}");
                }
            }));
            DebugManager.LogVerbose<EventReactiveNode>($"Initialized with eventKey: {eventKey}");
        }

        public NodeState Execute()
        {
            DebugManager.LogVerbose<EventReactiveNode>("Executing...");
            //var blackboard = (successNode as GenericActionNode)?.blackboard ?? (failureNode as GenericActionNode)?.blackboard;

            if (lastEvent != null)
            {
                DebugManager.Log<EventReactiveNode>("Event detected, executing success node");
                lastEvent = null; // Reseta após reagir
                var state = successNode?.Execute() ?? NodeState.Success;
                DebugManager.Log<EventReactiveNode>($"Success node returned: {state}");
                return state;
            }

            DebugManager.LogVerbose<EventReactiveNode>("No event detected, executing failure node");
            var failureState = failureNode?.Execute() ?? NodeState.Failure;
            DebugManager.Log<EventReactiveNode>($"Failure node returned: {failureState}");
            return failureState;
        }
    }
}