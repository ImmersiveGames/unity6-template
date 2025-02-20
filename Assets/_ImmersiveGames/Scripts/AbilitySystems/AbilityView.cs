using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityView : MonoBehaviour {
        [SerializeField] private AbilityButton[] buttons;
        private readonly Key[] keys = { Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4, Key.Digit5 };

        private void Awake() {
            for (var i = 0; i < buttons.Length; i++) {
                if (i >= keys.Length) {
                    Debug.LogError("Not enough keycodes for the number of buttons.");
                    break;
                }
                buttons[i].Initialize(i, keys[i]);
            }
        }

        public void UpdateButtonSprites(IList<Ability> abilities) {
            for (var i = 0; i < buttons.Length; i++) {
                if (i < abilities.Count && abilities[i] != null) {
                    buttons[i].UpdateIcon(abilities[i].Data.icon);
                    buttons[i].gameObject.SetActive(true);
                } else {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }

        public AbilityButton[] GetButtons() => buttons;

        public void UpdateCooldownProgress(int index, float progress) {
            if (index >= 0 && index < buttons.Length) {
                buttons[index].UpdateCooldownProgress(progress);
            }
        }
    }
}