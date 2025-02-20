using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityButton : MonoBehaviour {
        [SerializeField] private Image radialImage;
        [SerializeField] private Image abilityIcon;
        [SerializeField] private Button button;
        private int Index { get; set; }
        private Key Key { get; set; }

        public event System.Action<int> OnButtonPressed = delegate { };

        private void Start() {
            radialImage.fillAmount = 0f; // Inicializa o fillAmount como 0
            button.onClick.AddListener(() => OnButtonPressed(Index));
        }

        private void Update() {
            if (Keyboard.current[Key].wasPressedThisFrame) {
                OnButtonPressed(Index);
            }
        }

        public void Initialize(int index, Key key) {
            Index = index;
            Key = key;
            radialImage.fillAmount = 0f; // Garante que o fillAmount esteja zerado na inicialização
        }

        public void UpdateIcon(Sprite newIcon) {
            abilityIcon.sprite = newIcon;
        }

        public void UpdateCooldownProgress(float progress) {
            radialImage.fillAmount = progress; // Atualiza apenas quando o cooldown está ativo
        }
    }
}