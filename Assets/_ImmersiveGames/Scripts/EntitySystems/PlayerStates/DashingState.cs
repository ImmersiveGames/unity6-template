using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public class DashingState: BaseState {
        public DashingState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(DashHash,CrossFadeDuration);
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}