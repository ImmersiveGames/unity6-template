using System;
using _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems;
using UnityEngine;
using UnityEngine.AI;

namespace _ImmersiveGames.Scripts.Utils.MediatorSystems.Tests {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Agent : MonoBehaviour, IVisitable {
        //private IGoapMultithared goapSystem;
        NavMeshAgent agent;
        private Mediator<Agent> mediator;
        public AgentStatus status;

        private void Start() {
            //goapSystem = ServiceLocator.For(this).Get<GoapFactory<Agent>>().CreateMultithared(this);
            agent = GetComponent<NavMeshAgent>();
            mediator = ServiceLocator.For(this).Get<Mediator<Agent>>();
        }

        private void Update() {
            //agent.Move(goapSystem.Update(Time.deltaTime));

            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                Send(new MessagePayload.Builder(this).WithContent("Minha Mensagem Hello!").Build(), IsNearby);
            }
        }

        private void OnDestroy() => mediator.Deregister(this);
        void Send(IVisitor message)=> mediator.Broadcast(this, message);
        void Send(IVisitor message, Func<Agent, bool> predicate) => mediator.Broadcast(this, message, predicate);

        private float radios = 6.0f;
        private Func<Agent, bool> IsNearby => target => Vector3.Distance(transform.position, target.transform.position) <= radios;
        public void Accept(IVisitor message) => message.Visit(this);
        //public void PlanActions(AgentAction goal) => goapSystem.PlanActions(goal);
    }

    public enum AgentStatus {
        Active,
        Reset
    }
}