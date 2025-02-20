using UnityEngine;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilitySystem : MonoBehaviour {
        [SerializeField] private AbilityView view;
        [SerializeField] private AbilityData[] startingAbilities;
        private AbilityController controller;

        private void Awake() {
            var cooldownManager = new AbilityCooldownManager();
            controller = new AbilityController.Builder()
                .WithAbilities(startingAbilities)
                .WithCooldownManager(cooldownManager)
                .Build(view);
        }

        private void Update() => controller.Update();

        private void OnDisable() {
            controller.OnDisable();
        }
    }
}