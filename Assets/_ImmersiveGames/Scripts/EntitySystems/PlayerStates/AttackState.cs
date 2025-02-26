using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems {
    public class AttackState : BaseState {
        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Animator.CrossFade(AttackHash, CrossFadeDuration);
            Player.Attack();
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}