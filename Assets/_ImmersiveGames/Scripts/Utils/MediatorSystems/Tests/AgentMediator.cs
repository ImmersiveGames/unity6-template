using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.MediatorSystems.Tests {
    public class AgentMediator : Mediator<Agent> {
        private void Awake() => ServiceLocator.Global.Register<Mediator<Agent>>(this);

        protected override bool MediatorConditionMet(Agent target) => target.status == AgentStatus.Active;

        protected override void OnRegistered(Agent entity) {
            base.OnRegistered(entity);
            Debug.Log($"{entity.name} has been registered");
            Broadcast(entity, new MessagePayload.Builder(entity).WithContent("Register").Build());
        }

        protected override void OnDeregistered(Agent entity) {
            base.OnDeregistered(entity);
            Debug.Log($"{entity.name} has been deregistered");
            Broadcast(entity, new MessagePayload.Builder(entity).WithContent("Unregister").Build());
        }
    }
}