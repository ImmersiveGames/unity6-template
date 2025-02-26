using System;
using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.GoapSystem {
    public class MoveStrategy : IActionStrategy {
        private readonly NavMeshAgent agent;
        private readonly Func<Vector3> destination;

        public bool CanPerform => !Complete;
        public bool Complete => agent.remainingDistance <= 2f && !agent.pathPending;

        public MoveStrategy(NavMeshAgent agent, Func<Vector3> destination) {
            this.agent = agent;
            this.destination = destination;
        }

        public void Start() => agent.SetDestination(destination());
        public void Stop() => agent.ResetPath();
    }
}