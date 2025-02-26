using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class EventEmitterTest : MonoBehaviour
    {
        [SerializeField] private BehaviorTreeSo behaviorTree;

        private EventBinding<IBehaviorEvent> eventBinding;
        private IBehaviorNode tree;

        private void Start()
        {
            eventBinding = new EventBinding<IBehaviorEvent>(e => DebugManager.Log<EventEmitterTest>($"Evento: {e.EventKey} | State: {e.State}"));
            EventBus<IBehaviorEvent>.Register(eventBinding);
            tree = behaviorTree.Build();
        }

        private void Update() => tree?.Execute();

        private void OnDestroy() => EventBus<IBehaviorEvent>.Unregister(eventBinding);
    }
    
    
}