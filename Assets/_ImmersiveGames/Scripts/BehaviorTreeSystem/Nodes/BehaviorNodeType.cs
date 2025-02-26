namespace _ImmersiveGames.Scripts.BehaviorTreeSystem {
    public enum BehaviorNodeType
    {
        Action,
        Sequence,
        Selector,
        RandomSelector,
        Parallel,
        Repeat,
        Animate,
        Delay,
        Condition,
        EventReactive,
        GlobalCondition,
        ParallelCondition,
        RepeatWhile,
        Restart,
        RandomSequence,      // Executa todos em ordem aleatória, falha ao primeiro falha
        RandomPersistentSelector,
    }
}