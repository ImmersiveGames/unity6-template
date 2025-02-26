using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.BusEventSystems;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public interface IBehaviorEvent : IEvent
    {
        string EventKey { get; }
        BlackboardSo Blackboard { get; }
        NodeState? State { get; }
    }
}