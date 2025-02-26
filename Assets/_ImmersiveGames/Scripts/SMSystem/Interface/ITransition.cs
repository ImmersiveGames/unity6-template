namespace _ImmersiveGames.Scripts.SMSystem {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}