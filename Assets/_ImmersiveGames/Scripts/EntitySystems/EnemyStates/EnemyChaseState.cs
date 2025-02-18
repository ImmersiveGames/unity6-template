﻿using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.EntitySystems.EnemyStates {
    public class EnemyChaseState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Transform _player;
        
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            _agent = agent;
            _player = player;
        }
        
        public override void OnEnter() {
            base.OnEnter();
            // animação de correr animator.CrossFade(RunHash, crossFadeDuration);
        }
        
        public override void Update() {
            _agent.SetDestination(_player.position);
        }
    }
}