using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.EntitySystems.EnemyStates {
    public class EnemyWanderState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Vector3 _startPoint;
        private readonly float _wanderRadius;

        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy,
            animator) {
            _agent = agent;
            _startPoint = enemy.transform.position;
            _wanderRadius = wanderRadius;
        }

        public override void OnEnter() {
            base.OnEnter();
            Animator.CrossFade(LocomotionHash, CROSS_FADE_DURATION);
        }

        public override void Update() {
            if (!HasReachedDestination()) return;
            var randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection += _startPoint;
            NavMesh.SamplePosition(randomDirection, out var hit, _wanderRadius, 1);
            var finalPosition = hit.position;
                
            _agent.SetDestination(finalPosition);
        }

        private bool HasReachedDestination() {
            return !_agent.pathPending
                   && _agent.remainingDistance <= _agent.stoppingDistance
                   && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }
    }
}