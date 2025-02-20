using System.Collections.Generic;
using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using UnityEngine.Events;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityCooldownManager {
        private readonly Dictionary<int, CountdownTimer> cooldownTimers = new();
        public readonly UnityEvent<int, float> OnCooldownProgressChanged = new();

        public void StartCooldown(int abilityIndex, float duration) {
            if (!cooldownTimers.ContainsKey(abilityIndex)) {
                var timer = new CountdownTimer(duration);
                timer.OnTimerStop += () => {
                    cooldownTimers.Remove(abilityIndex);
                    OnCooldownProgressChanged.Invoke(abilityIndex, 0f); // Zera o fillAmount
                };
                cooldownTimers[abilityIndex] = timer;
            }
            cooldownTimers[abilityIndex].Reset(duration);
            cooldownTimers[abilityIndex].Start();
        }

        public void Update() {
            foreach (var (key, timer) in cooldownTimers) {
                if (timer.IsRunning) {
                    OnCooldownProgressChanged.Invoke(key, timer.Progress);
                }
            }
        }

        public bool IsOnCooldown(int abilityIndex) {
            return cooldownTimers.ContainsKey(abilityIndex) && cooldownTimers[abilityIndex].IsRunning;
        }

        public void Clear() {
            foreach (var timer in cooldownTimers.Values) {
                timer.Dispose();
            }
            cooldownTimers.Clear();
        }
    }
}