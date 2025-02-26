using _ImmersiveGames.Scripts.AdvancedTimers;

namespace _ImmersiveGames.Scripts.GoapSystem {
    public class IdleStrategy : IActionStrategy {
        public bool CanPerform => true; // Agent can always Idle
        public bool Complete { get; private set; }

        private readonly CountdownTimer timer;

        public IdleStrategy(float duration) {
            timer = new CountdownTimer(duration);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
        }

        public void Start() => timer.Start();
    }
}