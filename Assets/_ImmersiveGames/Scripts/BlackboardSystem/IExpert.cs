using _ImmersiveGames.Scripts.BlackboardSystem.BlackboardSystem;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    public interface IExpert {
        int GetInsistence(Blackboard blackboard);
        void Execute(Blackboard blackboard);
    }
    
}