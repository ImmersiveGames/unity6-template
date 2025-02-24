using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Events {
    public interface IBehaviorEvent : Utils.BusEventSystems.IEvent
    {
        string EventKey { get; }
        BlackboardSo Blackboard { get; }
        NodeState? State { get; }
    }
}