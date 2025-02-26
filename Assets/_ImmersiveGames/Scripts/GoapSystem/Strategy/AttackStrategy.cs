using _ImmersiveGames.Scripts.AdvancedTimers;

namespace _ImmersiveGames.Scripts.GoapSystem {
    public class AttackStrategy : IActionStrategy {
        public bool CanPerform => true; // Agent can always attack
        public bool Complete { get; private set; }

        private readonly CountdownTimer timer;
        //readonly AnimationController animations;

        //public AttackStrategy(AnimationController animations) {
        public AttackStrategy() {
            timer = new CountdownTimer(1.5f);
            /*this.animations = animations;
            timer = new CountdownTimer(animations.GetAnimationLength(animations.attackClip));
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;*/
        }

        public void Start() {
            timer.Start();
            //animations.Attack();
        }
    }
}