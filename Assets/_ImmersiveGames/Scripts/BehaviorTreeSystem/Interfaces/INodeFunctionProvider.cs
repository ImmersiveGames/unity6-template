using System;
using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces {
    public interface INodeFunctionProvider
    {
        Func<NodeState> GetNodeFunction();
        string NodeName { get; }
        int NodeID { get; }
    }
}