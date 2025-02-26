using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems {
    public class JumpState  : BaseState {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(JumpHash,CrossFadeDuration);
        }

        public override void FixedUpdate() {
            Player.HandleJump();
            Player.HandleMovement();
        }
    }
}