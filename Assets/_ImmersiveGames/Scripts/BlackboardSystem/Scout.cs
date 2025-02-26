using _ImmersiveGames.Scripts.BlackboardSystem.BlackboardSystem;
using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    public class Scout : MonoBehaviour, IExpert {
        private BlackboardKey isSafeKey;
        private bool dangerSensor;

        private void Start() {
            var controller = ServiceLocator.For(this).Get<BlackboardController>();
            var blackboard = controller.GetBlackboard();
            controller.RegisterExpert(this);
            isSafeKey = blackboard.GetOrRegisterKey("IsSafe");
        }

        public int GetInsistence(Blackboard blackboard) {
            return dangerSensor ? 100 : 0;
        }

        public void Execute(Blackboard blackboard) {
            blackboard.AddAction(() => {
                if (blackboard.TryGetValue(isSafeKey, out bool isSafe)) {
                    blackboard.SetValue(isSafeKey, !isSafe);
                }
                dangerSensor = false;
            });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                dangerSensor = !dangerSensor;
            }
        }
    }
}