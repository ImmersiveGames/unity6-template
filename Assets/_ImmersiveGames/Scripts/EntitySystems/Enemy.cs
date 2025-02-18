using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using _ImmersiveGames.Scripts.EntitySystems.EnemyStates;
using _ImmersiveGames.Scripts.EntitySystems.PlayerDetectorSystem;
using _ImmersiveGames.Scripts.SMSystem;
using _ImmersiveGames.Scripts.SMSystem.Interface;
using _ImmersiveGames.Scripts.SMSystem.Predicates;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.EntitySystems {
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity {
        [SerializeField, Self] private NavMeshAgent agent;
        [SerializeField, Self] private PlayerDetector playerDetector;
        [SerializeField, Child] private Animator animator;
        
        [SerializeField] private float wanderRadius = 10f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private StateMachine _stateMachine;

        private CountdownTimer _attackTimer;

        private void OnValidate() => this.ValidateRefs();

        private void Start() {
            _attackTimer = new CountdownTimer(timeBetweenAttacks);
            
            _stateMachine = new StateMachine();
            
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, playerDetector.Player);
            var attackState = new EnemyAttackState(this, animator, agent, playerDetector.Player);
            
            At(wanderState, chaseState, new FuncPredicate(() => playerDetector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(() => !playerDetector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => playerDetector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !playerDetector.CanAttackPlayer()));

            _stateMachine.SetState(wanderState);
        }

        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);

        private void Update() {
            _stateMachine.Update();
        }

        private void FixedUpdate() {
            _stateMachine.FixedUpdate();
        }
        
        public void Attack() {
            if (_attackTimer.IsRunning) return;
            
            _attackTimer.Start();
            playerDetector.PlayerHealth.TakeDamage(10);
        }
    }
}