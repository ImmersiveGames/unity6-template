using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public class LocomotionState  : BaseState {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(LocomotionHash,CrossFadeDuration);
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
    
}