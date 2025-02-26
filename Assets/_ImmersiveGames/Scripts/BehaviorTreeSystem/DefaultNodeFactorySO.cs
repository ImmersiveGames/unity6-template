using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem._ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem
{
    [CreateAssetMenu(fileName = "DefaultNodeFactory", menuName = "ImmersiveGames/Behavior/Factories/DefaultNodeFactory")]
    public class DefaultNodeFactorySo : ScriptableObject, INodeFactory
    {
        public IBehaviorNode CreateNode(BehaviorNodeType nodeType, NodeConfig config, BlackboardSo blackboard)
        {
            switch (nodeType)
            {
                case BehaviorNodeType.Action:
                    return config.strategy != null
                        ? new GenericActionNode(blackboard, config.strategy)
                        : null;

                case BehaviorNodeType.Sequence:
                    return new SequenceNode();

                case BehaviorNodeType.Selector:
                    return new Selector();

                case BehaviorNodeType.RandomSelector:
                    return new RandomSelector(config.maxTimes);

                case BehaviorNodeType.Parallel:
                    return new Parallel(config.requireAllSuccess, config.interruptOnSuccess);

                case BehaviorNodeType.Repeat:
                    return config.strategy != null
                        ? new Repeat(new GenericActionNode(blackboard, config.strategy), config.repeatTimes)
                        : null;

                case BehaviorNodeType.Animate:
                    return config.strategy != null
                        ? new AnimationDecorator(new GenericActionNode(blackboard, config.strategy),
                            config.animationTrigger, config.duration)
                        : null;

                case BehaviorNodeType.Delay:
                    return config.strategy != null
                        ? new DelayDecorator(new GenericActionNode(blackboard, config.strategy), config.delay)
                        : null;

                case BehaviorNodeType.Condition:
                    return BuildConditionNode(config, blackboard);

                case BehaviorNodeType.EventReactive:
                    return BuildEventReactiveNode(config, blackboard);

                case BehaviorNodeType.GlobalCondition:
                    return BuildGlobalConditionNode(config, blackboard);

                case BehaviorNodeType.ParallelCondition:
                    return BuildParallelConditionNode(config, blackboard);

                case BehaviorNodeType.RepeatWhile:
                    return new RepeatWhileDecorator(
                        config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null,
                        config.repeatCondition, blackboard);

                case BehaviorNodeType.Restart:
                    return new RestartDecorator(
                        config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null,
                        config.repeatCondition, blackboard);

                case BehaviorNodeType.RandomSequence:
                    return new RandomSequence();

                case BehaviorNodeType.RandomPersistentSelector:
                    return new RandomPersistentSelector();

                default:
                    throw new Exception($"Unknown node type: {nodeType}");
            }
        }

        private IBehaviorNode BuildConditionNode(NodeConfig config, BlackboardSo blackboard)
        {
            var successNode = config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null;
            var failureNode = config.children.Count > 1 ? config.children[1].Build(blackboard, this) : null;
            return new ConditionNode(config.condition, successNode, failureNode, blackboard);
        }

        private IBehaviorNode BuildEventReactiveNode(NodeConfig config, BlackboardSo blackboard)
        {
            var successNode = config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null;
            var failureNode = config.children.Count > 1 ? config.children[1].Build(blackboard, this) : null;
            return new EventReactiveNode(config.eventKey, successNode, failureNode);
        }

        private IBehaviorNode BuildGlobalConditionNode(NodeConfig config, BlackboardSo blackboard)
        {
            var overrideNode = config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null;
            return new GlobalConditionDecorator(
                new GenericActionNode(blackboard, config.strategy),
                config.condition,
                overrideNode,
                blackboard);
        }

        private IBehaviorNode BuildParallelConditionNode(NodeConfig config, BlackboardSo blackboard)
        {
            var parallelAction = config.children.Count > 0 ? config.children[0].Build(blackboard, this) : null;
            return new ParallelConditionDecorator(
                new GenericActionNode(blackboard, config.strategy),
                config.condition,
                parallelAction,
                blackboard);
        }
    }
}