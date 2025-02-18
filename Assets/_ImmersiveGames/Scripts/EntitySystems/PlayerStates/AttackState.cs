using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public class AttackState : BaseState {
        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Animator.CrossFade(AttackHash, CROSS_FADE_DURATION);
            Player.Attack();
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}