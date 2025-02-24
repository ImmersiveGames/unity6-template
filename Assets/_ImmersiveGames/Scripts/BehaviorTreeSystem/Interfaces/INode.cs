using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces {
    public interface INode
    {
        void OnEnter();
        NodeState Tick();
        void OnExit();
    }
}