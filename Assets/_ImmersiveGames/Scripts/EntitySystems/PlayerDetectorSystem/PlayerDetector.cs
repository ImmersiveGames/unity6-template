using _ImmersiveGames.Scripts.AdvancedTimers;
using UnityEngine;

namespace _ImmersiveGames.Scripts.EntitySystems.PlayerDetectorSystem {
    public class PlayerDetector : MonoBehaviour {
        [SerializeField] private float detectionAngle = 60f; // Cone in front of enemy
        [SerializeField] private float detectionRadius = 10f; // Large circle around enemy
        [SerializeField] private float innerDetectionRadius = 5f; // Small circle around enemy
        [SerializeField] private float detectionCooldown = 1f; // Time between detections
        [SerializeField] private float attackRange = 2f; // Distance from enemy to player to attack
        
        public Transform Player { get; private set; }
        public Health PlayerHealth { get; private set; }

        private CountdownTimer _detectionTimer;

        private IDetectionStrategy _detectionStrategy;

        private void Awake() {
            Player = GameObject.FindGameObjectWithTag("Player").transform; // Make sure to TAG the player!
            PlayerHealth = Player.GetComponent<Health>();
        }

        private void Start() {
            _detectionTimer = new CountdownTimer(detectionCooldown);
            _detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }
        

        public bool CanDetectPlayer() {
            return _detectionTimer.IsRunning || _detectionStrategy.Execute(Player, transform, _detectionTimer);
        }

        public bool CanAttackPlayer() {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this._detectionStrategy = detectionStrategy;

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);

            // Calculate our cone directions
            var forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;
            var backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;

            // Draw lines to represent the cone
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }
    }
}