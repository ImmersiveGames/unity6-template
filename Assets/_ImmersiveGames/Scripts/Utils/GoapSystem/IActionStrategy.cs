namespace _ImmersiveGames.Scripts.Utils.GoapSystem {
    public interface IActionStrategy {
        bool CanPerform { get; }
        bool Complete { get; }
        void Start() { }
        void Update(float deltaTime) { }
        void Stop() { }
    }
}