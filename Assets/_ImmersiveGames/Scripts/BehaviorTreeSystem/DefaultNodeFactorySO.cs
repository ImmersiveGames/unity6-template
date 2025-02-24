using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes._ImmersiveGames.Scripts.BehaviorTreeSystem.Nodes;
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
                    return config.Strategy != null
                        ? new GenericActionNode(blackboard, config.Strategy)
                        : null;

                case BehaviorNodeType.Sequence:
                    return new SequenceNode();

                case BehaviorNodeType.Selector:
                    return new Selector();

                case BehaviorNodeType.RandomSelector:
                    return new RandomSelector(config.MaxTimes);

                case BehaviorNodeType.Parallel:
                    return new Parallel(config.RequireAllSuccess, config.InterruptOnSuccess);

                case BehaviorNodeType.Repeat:
                    return config.Strategy != null
                        ? new Repeat(new GenericActionNode(blackboard, config.Strategy), config.RepeatTimes)
                        : null;

                case BehaviorNodeType.Animate:
                    return config.Strategy != null
                        ? new AnimationDecorator(new GenericActionNode(blackboard, config.Strategy),
                            config.AnimationTrigger, config.Duration)
                        : null;

                case BehaviorNodeType.Delay:
                    return config.Strategy != null
                        ? new DelayDecorator(new GenericActionNode(blackboard, config.Strategy), config.Delay)
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
                        config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null,
                        config.RepeatCondition, blackboard);

                case BehaviorNodeType.Restart:
                    return new RestartDecorator(
                        config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null,
                        config.RepeatCondition, blackboard);

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
            var successNode = config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null;
            var failureNode = config.Children.Count > 1 ? config.Children[1].Build(blackboard, this) : null;
            return new ConditionNode(config.Condition, successNode, failureNode, blackboard);
        }

        private IBehaviorNode BuildEventReactiveNode(NodeConfig config, BlackboardSo blackboard)
        {
            var successNode = config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null;
            var failureNode = config.Children.Count > 1 ? config.Children[1].Build(blackboard, this) : null;
            return new EventReactiveNode(config.EventKey, successNode, failureNode);
        }

        private IBehaviorNode BuildGlobalConditionNode(NodeConfig config, BlackboardSo blackboard)
        {
            var overrideNode = config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null;
            return new GlobalConditionDecorator(
                new GenericActionNode(blackboard, config.Strategy),
                config.Condition,
                overrideNode,
                blackboard);
        }

        private IBehaviorNode BuildParallelConditionNode(NodeConfig config, BlackboardSo blackboard)
        {
            var parallelAction = config.Children.Count > 0 ? config.Children[0].Build(blackboard, this) : null;
            return new ParallelConditionDecorator(
                new GenericActionNode(blackboard, config.Strategy),
                config.Condition,
                parallelAction,
                blackboard);
        }
    }
}