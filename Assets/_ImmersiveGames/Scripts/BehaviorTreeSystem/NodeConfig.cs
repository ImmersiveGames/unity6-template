using System;
using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Events;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Strategies;
using _ImmersiveGames.Scripts.Utils.BusEventSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [Serializable]
    public class NodeConfig
    {
        [FoldoutGroup("Node Settings"), EnumPaging]
        public BehaviorNodeType NodeType;

        [FoldoutGroup("Node Settings"), ShowIf("RequiresStrategy"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ActionStrategySo Strategy;

        [FoldoutGroup("Parameters"), ShowIf("HasRepeatTimes")]
        public int RepeatTimes = 1;

        [FoldoutGroup("Parameters"), ShowIf("HasDuration")]
        public float Duration = 1f;

        [FoldoutGroup("Parameters"), ShowIf("HasMaxTimes")]
        public int MaxTimes = 1;

        [FoldoutGroup("Parameters"), ShowIf("HasParallelParams")]
        public bool RequireAllSuccess = true;

        [FoldoutGroup("Parameters"), ShowIf("HasParallelParams")]
        public bool InterruptOnSuccess;

        [FoldoutGroup("Parameters"), ShowIf("HasDelay"), LabelText("Delay Time")]
        public float Delay = 1f;

        [FoldoutGroup("Parameters"), ShowIf("HasEventKey")]
        public string EventKey;

        [FoldoutGroup("Parameters"), ShowIf("HasAnimationTrigger")]
        public string AnimationTrigger;

        [FoldoutGroup("Enter Exit Events"), LabelText("Use Enter/Exit Events")]
        public bool HasEnterExit;

        [FoldoutGroup("Enter Exit Events"), ShowIf("HasEnterExit"), LabelText("Enter Event Key")]
        public string EnterEventKey;

        [FoldoutGroup("Enter Exit Events"), ShowIf("HasEnterExit"), LabelText("Exit Event Key")]
        public string ExitEventKey;

        [FoldoutGroup("Condition"), ShowIf("HasCondition"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo Condition;

        [FoldoutGroup("Repeat While"), ShowIf("IsRepeatWhile"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo RepeatCondition;

        [FoldoutGroup("Restart Condition"), ShowIf("IsRestart"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo RestartCondition;

        [FoldoutGroup("Children"),
         ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true, DraggableItems = true, NumberOfItemsPerPage = 5)]
        [ShowIf("IsComposite")]
        public List<NodeConfig> Children = new List<NodeConfig>();

        public IBehaviorNode Build(BlackboardSo blackboard, INodeFactory factory)
        {
            if (factory == null)
            {
                Debug.LogError("INodeFactory nulo fornecido para construção do NodeConfig");
                return null;
            }

            IBehaviorNode node = factory.CreateNode(NodeType, this, blackboard);

            if (node == null)
            {
                Debug.LogWarning($"Nó {NodeType} não pôde ser construído (Strategy ou configuração possivelmente inválida)");
                return new GenericActionNode(blackboard, ScriptableObject.CreateInstance<MockDebugSo>());
            }

            if (NodeType != BehaviorNodeType.RepeatWhile && NodeType != BehaviorNodeType.Restart &&
                node is ICompositeNode composite)
            {
                foreach (var child in Children)
                {
                    if (child == null)
                    {
                        Debug.LogWarning($"Filho nulo encontrado na configuração do nó {NodeType}");
                        continue;
                    }
                    var childNode = child.Build(blackboard, factory);
                    if (childNode != null)
                    {
                        composite.AddChild(childNode);
                    }
                }
            }

            if (HasEnterExit)
            {
                node = new EnterExitDecorator(node, blackboard, EnterEventKey, ExitEventKey);
            }

            return node;
        }

        private bool RequiresStrategy() => StrategyNodes.Contains(NodeType);
        private bool IsComposite() => CompositeNodes.Contains(NodeType);
        private bool HasCondition() => ConditionNodes.Contains(NodeType);
        private bool HasRepeatTimes() => NodeType == BehaviorNodeType.Repeat;
        private bool HasDuration() => NodeType == BehaviorNodeType.Animate;
        private bool HasMaxTimes() => NodeType == BehaviorNodeType.RandomSelector;
        private bool HasParallelParams() => NodeType == BehaviorNodeType.Parallel;
        private bool HasDelay() => NodeType == BehaviorNodeType.Delay;
        private bool HasEventKey() => NodeType == BehaviorNodeType.EventReactive;
        private bool HasAnimationTrigger() => NodeType == BehaviorNodeType.Animate;
        private bool IsRepeatWhile() => NodeType == BehaviorNodeType.RepeatWhile;
        private bool IsRestart() => NodeType == BehaviorNodeType.Restart;

        private static readonly BehaviorNodeType[] StrategyNodes = new[]
        {
            BehaviorNodeType.Action, BehaviorNodeType.Repeat, BehaviorNodeType.Animate, BehaviorNodeType.Delay,
            BehaviorNodeType.GlobalCondition, BehaviorNodeType.ParallelCondition
        };

        private static readonly BehaviorNodeType[] CompositeNodes = new[]
        {
            BehaviorNodeType.Sequence, BehaviorNodeType.Selector, BehaviorNodeType.RandomSelector,
            BehaviorNodeType.Parallel, BehaviorNodeType.Condition, BehaviorNodeType.EventReactive,
            BehaviorNodeType.GlobalCondition, BehaviorNodeType.ParallelCondition,
            BehaviorNodeType.RandomSequence, BehaviorNodeType.RandomPersistentSelector
        };

        private static readonly BehaviorNodeType[] ConditionNodes = new[]
        {
            BehaviorNodeType.Condition, BehaviorNodeType.EventReactive, BehaviorNodeType.GlobalCondition,
            BehaviorNodeType.ParallelCondition, BehaviorNodeType.RepeatWhile
        };
    }
}