using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.EntitySystems.EnemyStates {
    public class EnemyAttackState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Transform _player;
        
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            _agent = agent;
            _player = player;
        }
        
        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(AttackHash, CrossFadeDuration);
        }
        
        public override void Update() {
            _agent.SetDestination(_player.position);
            Enemy.Attack();
        }
    }
}