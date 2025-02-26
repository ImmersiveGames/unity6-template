using System;
using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.DebugSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [Serializable]
    public class NodeConfig
    {
        [FoldoutGroup("Node Settings"), EnumPaging]
        public BehaviorNodeType nodeType;

        [FoldoutGroup("Node Settings"), ShowIf("RequiresStrategy"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ActionStrategySo strategy;

        [FoldoutGroup("Parameters"), ShowIf("HasRepeatTimes")]
        public int repeatTimes = 1;

        [FoldoutGroup("Parameters"), ShowIf("HasDuration")]
        public float duration = 1f;

        [FoldoutGroup("Parameters"), ShowIf("HasMaxTimes")]
        public int maxTimes = 1;

        [FoldoutGroup("Parameters"), ShowIf("HasParallelParams")]
        public bool requireAllSuccess = true;

        [FoldoutGroup("Parameters"), ShowIf("HasParallelParams")]
        public bool interruptOnSuccess;

       [FoldoutGroup("Parameters"), ShowIf("HasDelay"), LabelText("Delay Time")]
        public float delay = 1f;

        [FoldoutGroup("Parameters"), ShowIf("HasEventKey")]
        public string eventKey;
        
        [FoldoutGroup("Parameters"), ShowIf("HasAnimationTrigger")]
        public string animationTrigger;

        [FoldoutGroup("Enter Exit Events"), LabelText("Use Enter/Exit Events")]
        public bool hasEnterExit;

        [FoldoutGroup("Enter Exit Events"), ShowIf("hasEnterExit"), LabelText("Enter Event Key")]
        public string enterEventKey;

        [FoldoutGroup("Enter Exit Events"), ShowIf("hasEnterExit"), LabelText("Exit Event Key")]
        public string exitEventKey;

        [FoldoutGroup("Condition"), ShowIf("HasCondition"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo condition;

        [FoldoutGroup("Repeat While"), ShowIf("IsRepeatWhile"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo repeatCondition;

        [FoldoutGroup("Restart Condition"), ShowIf("IsRestart"), InlineEditor(InlineEditorModes.GUIAndHeader)]
        public ConditionStrategySo restartCondition;
        
        [FoldoutGroup("Children"),
         ListDrawerSettings(ShowIndexLabels = true, ShowItemCount = true, DraggableItems = true, NumberOfItemsPerPage = 5)]
        [ShowIf("IsComposite")]
        public List<NodeConfig> children = new ();

        public IBehaviorNode Build(BlackboardSo blackboard, INodeFactory factory)
        {
            if (factory == null)
            {
                DebugManager.LogError<NodeConfig>("INodeFactory nulo fornecido para construção do NodeConfig");
                return null;
            }

            var node = factory.CreateNode(nodeType, this, blackboard);

            if (node == null)
            {
                DebugManager.LogWarning<NodeConfig>($"Nó {nodeType} não pôde ser construído (Strategy ou configuração possivelmente inválida)");
                return new GenericActionNode(blackboard, ScriptableObject.CreateInstance<MockDebugSo>());
            }

            if (nodeType != BehaviorNodeType.RepeatWhile && nodeType != BehaviorNodeType.Restart &&
                node is ICompositeNode composite)
            {
                foreach (var child in children)
                {
                    if (child == null)
                    {
                        DebugManager.LogWarning<NodeConfig>($"Filho nulo encontrado na configuração do nó {nodeType}");
                        continue;
                    }
                    var childNode = child.Build(blackboard, factory);
                    if (childNode != null)
                    {
                        composite.AddChild(childNode);
                    }
                }
            }

            if (hasEnterExit)
            {
                node = new EnterExitDecorator(node, blackboard, enterEventKey, exitEventKey);
            }

            return node;
        }

        private bool RequiresStrategy() => StrategyNodes.Contains(nodeType);
        private bool IsComposite() => CompositeNodes.Contains(nodeType);
        private bool HasCondition() => ConditionNodes.Contains(nodeType);
        private bool HasRepeatTimes() => nodeType == BehaviorNodeType.Repeat;
        private bool HasDuration() => nodeType == BehaviorNodeType.Animate;
        private bool HasMaxTimes() => nodeType == BehaviorNodeType.RandomSelector;
        private bool HasParallelParams() => nodeType == BehaviorNodeType.Parallel;
        private bool HasDelay() => nodeType == BehaviorNodeType.Delay;
        private bool HasEventKey() => nodeType == BehaviorNodeType.EventReactive;
        private bool HasAnimationTrigger() => nodeType == BehaviorNodeType.Animate;
        private bool IsRepeatWhile() => nodeType == BehaviorNodeType.RepeatWhile;
        private bool IsRestart() => nodeType == BehaviorNodeType.Restart;

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