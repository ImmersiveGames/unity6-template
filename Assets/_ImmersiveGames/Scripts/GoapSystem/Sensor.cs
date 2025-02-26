using System;
using _ImmersiveGames.Scripts.AdvancedTimers;
using _ImmersiveGames.Scripts.Utils.Extensions;
using UnityEngine;

namespace _ImmersiveGames.Scripts.GoapSystem {
    [RequireComponent(typeof(SphereCollider))]
    public class Sensor : MonoBehaviour {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private float timerInterval = 1f;

        private SphereCollider detectionRange;
    
        public event Action OnTargetChanged = delegate { };
    
        public Vector3 TargetPosition => target ? target.transform.position : Vector3.zero;
        public bool IsTargetInRange => TargetPosition != Vector3.zero;

        private GameObject target;
        private Vector3 lastKnownPosition;
        private CountdownTimer timer;

        private void Awake() {
            detectionRange = GetComponent<SphereCollider>();
            detectionRange.isTrigger = true;
            detectionRange.radius = detectionRadius;
        }

        private void Start() {
            timer = new CountdownTimer(timerInterval);
            timer.OnTimerStop += () => {
                UpdateTargetPosition(target.OrNull());
                timer.Start();
            };
            timer.Start();
        }

        private void UpdateTargetPosition(GameObject objTarget = null) {
            target = objTarget;
            if (IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero)) {
                lastKnownPosition = TargetPosition;
                OnTargetChanged.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return; //TODO: Melhorar a detecção
            UpdateTargetPosition(other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;
            UpdateTargetPosition();
        }

        private void OnDrawGizmos() {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}