using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public class LocomotionState  : BaseState {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(LocomotionHash,CROSS_FADE_DURATION);
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
    
}