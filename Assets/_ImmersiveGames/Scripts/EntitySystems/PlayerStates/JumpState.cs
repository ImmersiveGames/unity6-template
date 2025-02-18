using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public class JumpState  : BaseState {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(JumpHash,CROSS_FADE_DURATION);
        }

        public override void FixedUpdate() {
            Player.HandleJump();
            Player.HandleMovement();
        }
    }
}