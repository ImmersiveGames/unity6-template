using _ImmersiveGames.Scripts.BehaviorTreeSystem.Core;

namespace _ImmersiveGames.Scripts.BehaviorTreeSystem.Interfaces {
    public interface IActionStrategy<T>
    {
        NodeState ExecuteAction(T agent);
    }
}