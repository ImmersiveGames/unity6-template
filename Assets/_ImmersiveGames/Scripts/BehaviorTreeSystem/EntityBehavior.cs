using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class EntityBehavior : MonoBehaviour
    {
        [SerializeField, InlineEditor(InlineEditorModes.GUIAndHeader)]
        private BlackboardSo blackboard;

        [SerializeField, InlineEditor(InlineEditorModes.FullEditor)]
        private BehaviorTreeSo tree;

        [SerializeField]
        private Transform target;

        private IBehaviorNode root;
        private EventBinding<IBehaviorEvent> eventBinding;
        private bool isCompleted;

        private void Awake()
        {
            DebugManager.LogVerbose<EntityBehavior>("Awake called, initializing blackboard...");
            blackboard.Reset();
            blackboard.Initialize(gameObject);
            if (target != null)
            {
                blackboard.Target = target;
                DebugManager.Log<EntityBehavior>($"Target assigned: {target.name}");
            }

            eventBinding = new EventBinding<IBehaviorEvent>(OnBehaviorEvent);
            EventBus<IBehaviorEvent>.Register(eventBinding);
            DebugManager.LogVerbose<EntityBehavior>("EventBus listener registered");
        }

        private void Start()
        {
            DebugManager.LogVerbose<EntityBehavior>("Start called, building Behavior Tree...");
            root = tree.Build(blackboard);
            DebugManager.Log<EntityBehavior>($"Root constructed: {root != null}, RootNodes: {tree.rootNodes?.Count ?? 0}");
        }

        private void Update()
        {
            if (!isCompleted && root != null)
            {
                DebugManager.LogVerbose<EntityBehavior>("Updating Behavior Tree...");
                blackboard.Position = transform.position;
                var state = root.Execute();
                DebugManager.Log<EntityBehavior>($"Root state: {state}");
                if (state != NodeState.Success && state != NodeState.Failure) return;
                isCompleted = true;
                DebugManager.Log<EntityBehavior>("Execution completed");
            }
            else
            {
                DebugManager.LogVerbose<EntityBehavior>($"Not executed: {(isCompleted ? "Already completed" : "Root is null")}");
            }
        }

        private void OnDestroy() {
            if (eventBinding == null) return;
            EventBus<IBehaviorEvent>.Unregister(eventBinding);
            DebugManager.LogVerbose<EntityBehavior>("EventBus listener unregistered");
        }

        private static void OnBehaviorEvent(IBehaviorEvent evt)
        {
            DebugManager.Log<EntityBehavior>($"Event received: {evt.EventKey} | State: {evt.State}");
        }
    }
}