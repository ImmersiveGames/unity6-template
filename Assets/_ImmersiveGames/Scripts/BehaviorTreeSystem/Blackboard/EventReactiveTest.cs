using _ImmersiveGames.Scripts.BehaviorTreeSystem.Events;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.Utils.BusEventSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Blackboard
{
    public class EventReactiveTest : MonoBehaviour
    {
        [SerializeField, InlineEditor(InlineEditorModes.GUIAndHeader)]
        private BlackboardSo blackboard;

        [SerializeField, InlineEditor(InlineEditorModes.FullEditor)]
        private BehaviorTreeSo behaviorTree;

        [SerializeField]
        private Transform target;

        [SerializeField, Button("Emit Test Event")]
        private void EmitTestEvent()
        {
            EventBus<IBehaviorEvent>.Raise(new BehaviorEvent("TestReaction", blackboard, null));
            Debug.Log("Evento 'TestReaction' emitido");
        }

        private IBehaviorNode root;
        private EventBinding<IBehaviorEvent> eventBinding;
        private bool isCompleted = false;

        void Awake()
        {
            blackboard.Reset();
            blackboard.Initialize(gameObject);
            if (target != null)
                blackboard.Target = target;

            eventBinding = new EventBinding<IBehaviorEvent>(OnBehaviorEvent);
            EventBus<IBehaviorEvent>.Register(eventBinding);
        }

        void Start()
        {
            root = behaviorTree.Build(blackboard);
            Debug.Log($"Root construída: {root != null}, RootNodes: {behaviorTree.rootNodes?.Count ?? 0}");
        }

        void Update()
        {
            if (!isCompleted && root != null)
            {
                blackboard.Position = transform.position;
                NodeState state = root.Execute();
                Debug.Log($"Estado da raiz: {state}");
                if (state == NodeState.Success || state == NodeState.Failure)
                {
                    isCompleted = true;
                    Debug.Log("Execução concluída");
                }
            }
            else
            {
                Debug.Log("Não executado: " + (isCompleted ? "Já concluído" : "Root nula"));
            }
        }

        void OnDestroy()
        {
            if (eventBinding != null)
                EventBus<IBehaviorEvent>.Unregister(eventBinding);
        }

        private void OnBehaviorEvent(IBehaviorEvent evt)
        {
            Debug.Log($"Evento recebido: {evt.EventKey} | State: {evt.State}");
        }
    }
}