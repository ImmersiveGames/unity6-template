using UnityEngine;
using _ImmersiveGames.Scripts.Utils.BusEventSystems;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityActionHandler : MonoBehaviour {
        private EventBinding<AbilityActionEvent> _actionBinding;

        private void Awake() {
            // Inicializa o binding com o método que trata o evento
            _actionBinding = new EventBinding<AbilityActionEvent>(OnAbilityAction);
            EventBus<AbilityActionEvent>.Register(_actionBinding);
        }

        private void OnDestroy() {
            // Remove o binding ao destruir o objeto
            EventBus<AbilityActionEvent>.Unregister(_actionBinding);
        }

        private void OnAbilityAction(AbilityActionEvent evt) {
            // Log básico para depuração
            Debug.Log($"Habilidade '{evt.AbilityData.fullName}' (índice {evt.AbilityIndex}) ativada por {evt.Source ?? "desconhecido"}!");

            // Exemplo: Disparar animação (mantendo compatibilidade com PlayerAnimationEvent)
            if (evt.AbilityData.animationClip != null) {
                EventBus<PlayerAnimationEvent>.Raise(new PlayerAnimationEvent {
                    AnimationHash = evt.AbilityData.animationHash
                });
            }

            // Exemplo: Tocar um som (descomente e adicione 'soundName' ao AbilityData se desejar)
            // if (evt.AbilityData.soundName != null) {
            //     AudioManager.Play(evt.AbilityData.soundName);
            // }

            // Exemplo: Instanciar um efeito visual (descomente e adicione 'vfxPrefab' ao AbilityData se desejar)
            // if (evt.AbilityData.vfxPrefab != null) {
            //     Instantiate(evt.AbilityData.vfxPrefab, transform.position, Quaternion.identity);
            // }
        }
    }
    
}