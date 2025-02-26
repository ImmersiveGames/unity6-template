using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.DebugSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    public class EventReactiveTest : MonoBehaviour
    {
        [SerializeField, InlineEditor(InlineEditorModes.GUIAndHeader)]
        private BlackboardSo blackboard;

        [SerializeField, InlineEditor(InlineEditorModes.FullEditor)]
        private BehaviorTreeSo behaviorTree;

        [SerializeField]
        private Transform target;

        [Button("Emit Test Event")]
        private void EmitTestEvent()
        {
            EventBus<IBehaviorEvent>.Raise(new BehaviorEvent("TestReaction", blackboard, null));
            DebugManager.Log<EventReactiveTest>("Evento 'TestReaction' emitido");
        }

        private IBehaviorNode root;
        private EventBinding<IBehaviorEvent> eventBinding;
        private bool isCompleted;

        private void Awake()
        {
            blackboard.Reset();
            blackboard.Initialize(gameObject);
            if (target != null)
                blackboard.Target = target;

            eventBinding = new EventBinding<IBehaviorEvent>(OnBehaviorEvent);
            EventBus<IBehaviorEvent>.Register(eventBinding);
        }

        private void Start()
        {
            root = behaviorTree.Build(blackboard);
            DebugManager.Log<EventReactiveTest>($"Root construída: {root != null}, RootNodes: {behaviorTree.rootNodes?.Count ?? 0}");
        }

        private void Update()
        {
            if (!isCompleted && root != null)
            {
                blackboard.Position = transform.position;
                var state = root.Execute();
                DebugManager.Log<EventReactiveTest>($"Estado da raiz: {state}");
                if (state != NodeState.Success && state != NodeState.Failure) return;
                isCompleted = true;
                DebugManager.Log<EventReactiveTest>("Execução concluída");
            }
            else
            {
                DebugManager.Log<EventReactiveTest>("Não executado: " + (isCompleted ? "Já concluído" : "Root nula"));
            }
        }

        private void OnDestroy()
        {
            if (eventBinding != null)
                EventBus<IBehaviorEvent>.Unregister(eventBinding);
        }

        private void OnBehaviorEvent(IBehaviorEvent evt)
        {
            DebugManager.Log<EventReactiveTest>($"Evento recebido: {evt.EventKey} | State: {evt.State}");
        }
    }
}