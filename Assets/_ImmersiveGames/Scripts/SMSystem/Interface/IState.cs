namespace _ImmersiveGames.Scripts.SMSystem {
    public interface IState {
        void Update();
        void FixedUpdate();
        void OnEnter();
        void OnExit();
    }
}