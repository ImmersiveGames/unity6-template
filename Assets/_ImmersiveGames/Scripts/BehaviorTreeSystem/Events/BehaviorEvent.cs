using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class BehaviorEvent : IBehaviorEvent
    {
        public string EventKey { get; }
        public BlackboardSo Blackboard { get; }
        public NodeState? State { get; }

        public BehaviorEvent(string eventKey, BlackboardSo blackboard, NodeState? state = null)
        {
            EventKey = eventKey;
            Blackboard = blackboard;
            State = state;
        }
    }
}