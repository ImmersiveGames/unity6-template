using _ImmersiveGames.Scripts.BehaviorTreeSystem.Events;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.Utils.BusEventSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Blackboard
{
    public class EventEmitterTest : MonoBehaviour
    {
        [SerializeField] private BehaviorTreeSo behaviorTree;

        private EventBinding<IBehaviorEvent> eventBinding;
        private IBehaviorNode tree;

        void Start()
        {
            eventBinding = new EventBinding<IBehaviorEvent>(e => Debug.Log($"Evento: {e.EventKey} | State: {e.State}"));
            EventBus<IBehaviorEvent>.Register(eventBinding);
            tree = behaviorTree.Build();
        }

        void Update()
        {
            if (tree != null)
            {
                tree.Execute();
            }
        }

        void OnDestroy()
        {
            EventBus<IBehaviorEvent>.Unregister(eventBinding);
        }
    }
    
    
}