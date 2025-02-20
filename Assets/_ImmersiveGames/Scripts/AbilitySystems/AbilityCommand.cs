// TODO: Considerar implementar uma abordagem híbrida para eventos de habilidade:
// - Manter o AbilityActionEvent como evento centralizado para disparo inicial.
// - Adicionar um AbilityActionDispatcher que escuta AbilityActionEvent e dispara sub eventos específicos
//   (ex.: PlayerAnimationEvent, PlayerAudioEvent, PlayerVfxEvent) para cada tipo de ação.
// - Vantagens: Combina flexibilidade do evento centralizado com a separação clara de múltiplos eventos.
// - Prompt para recordar: "Implementar solução híbrida de eventos para o sistema de habilidades com AbilityActionDispatcher."

using _ImmersiveGames.Scripts.Utils.BusEventSystems;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityCommand : ICommand {
        private readonly AbilityData data;
        private readonly int index;
        private readonly object source;
        public float Duration => data.duration;

        public AbilityCommand(AbilityData data, int index, object source = null) {
            this.data = data;
            this.index = index;
            this.source = source;
        }

        public void Execute() {
            EventBus<AbilityActionEvent>.Raise(new AbilityActionEvent {
                AbilityData = data,
                AbilityIndex = index,
                Source = source
            });
        }
    }
}