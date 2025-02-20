using System;
using System.Collections.Generic;
using _ImmersiveGames.Scripts.Utils;
using UnityEngine.EventSystems;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityController {
        private readonly AbilityModel model;
        private readonly AbilityView view;
        private readonly AbilityCooldownManager cooldownManager;
        private readonly Queue<AbilityCommand> abilityQueue = new();

        private AbilityController(AbilityView view, AbilityModel model, AbilityCooldownManager cooldownManager) {
            this.view = view;
            this.model = model;
            this.cooldownManager = cooldownManager;

            ConnectModel();
            ConnectView();
            ConnectCooldownManager();
        }

        private void ConnectModel() {
            model.Abilities.AnyValueChanged += UpdateButtons;
        }

        private void ConnectView() {
            var buttons = view.GetButtons();
            for (var i = 0; i < buttons.Length; i++) {
                buttons[i].OnButtonPressed += OnAbilityButtonPressed;
            }
            view.UpdateButtonSprites(model.Abilities);
        }

        private void ConnectCooldownManager() {
            cooldownManager.OnCooldownProgressChanged.AddListener(view.UpdateCooldownProgress);
        }

        public void Update() {
            cooldownManager.Update();
            if (abilityQueue.Count <= 0 || IsAnyAbilityOnCooldown()) return;
            var cmd = abilityQueue.Dequeue();
            cmd.Execute();
        }

        private void UpdateButtons(IList<Ability> updatedAbilities) => view.UpdateButtonSprites(updatedAbilities);

        private void OnAbilityButtonPressed(int index) {
            if (model.Abilities[index] != null && !cooldownManager.IsOnCooldown(index)) {
                var cmd = model.Abilities[index].CreateCommand(index, source: this);
                abilityQueue.Enqueue(cmd);
                cooldownManager.StartCooldown(index, cmd.Duration);
            }
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnDisable() {
            model.Abilities.AnyValueChanged -= UpdateButtons;
            var buttons = view.GetButtons();
            for (var i = 0; i < buttons.Length; i++) {
                buttons[i].OnButtonPressed -= OnAbilityButtonPressed;
            }
            cooldownManager.OnCooldownProgressChanged.RemoveListener(view.UpdateCooldownProgress);
            cooldownManager.Clear();
            abilityQueue.Clear();
        }

        private bool IsAnyAbilityOnCooldown() {
            for (var i = 0; i < model.Abilities.Count; i++) {
                if (cooldownManager.IsOnCooldown(i)) return true;
            }
            return false;
        }

        public class Builder {
            private readonly AbilityModel model = new AbilityModel();
            private AbilityCooldownManager cooldownManager;

            public Builder WithAbilities(AbilityData[] datas) {
                foreach (var data in datas) {
                    model.Add(new Ability(data));
                }
                return this;
            }

            public Builder WithCooldownManager(AbilityCooldownManager manager) {
                cooldownManager = manager ?? throw new ArgumentNullException(nameof(manager));
                return this;
            }

            public AbilityController Build(AbilityView view) {
                Preconditions.CheckNotNull(view);
                Preconditions.CheckNotNull(cooldownManager, "CooldownManager must be provided via WithCooldownManager.");
                return new AbilityController(view, model, cooldownManager);
            }
        }
    }
}