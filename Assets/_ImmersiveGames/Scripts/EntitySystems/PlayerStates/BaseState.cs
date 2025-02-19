using _ImmersiveGames.Scripts.SMSystem.Interface;
using _ImmersiveGames.Scripts.Utils.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerStates {
    public abstract class BaseState: IState {
        protected readonly PlayerController Player;
        protected readonly Animator Animator;
        
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int DashHash = Animator.StringToHash("Dash");
        protected static readonly int AttackHash = Animator.StringToHash("Punch");
        
        protected const float CrossFadeDuration = 0.1f;
        
        protected BaseState(PlayerController player, Animator animator) {
            Player = player;
            Animator = animator;
        }
        
        public virtual void OnEnter() {
            // noop
            DebugManager.Log<BaseState>($"Enter state {GetType().Name}");
        }

        public virtual void Update() {
            // noop
        }

        public virtual void FixedUpdate() {
            // noop
        }

        public virtual void OnExit() {
            // noop
            DebugManager.Log<BaseState>($"Exiting state {GetType().Name}");
        }
    }
}