namespace _ImmersiveGames.Scripts.SMSystem.Interface {
    public interface ITransition {
        IState To { get; }
        IPredicate Condition { get; }
    }
}