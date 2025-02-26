using System;
using _ImmersiveGames.Scripts.BlackboardSystem.BlackboardSystem;
using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    public class BlackboardController : MonoBehaviour {
        [InlineEditor, SerializeField] private BlackboardData blackboardData;
        private readonly Blackboard blackboard = new Blackboard();
        private readonly Arbiter arbiter = new Arbiter();

        private void Awake() {
            ServiceLocator.Global.Register(this);
            blackboardData.SetValuesOnBlackboard(blackboard);
            blackboard.Debug();
        }
        
        public Blackboard GetBlackboard() => blackboard;
        
        public void RegisterExpert(IExpert expert) => arbiter.RegisterExpert(expert);
        public void DeregisterExpert(IExpert expert) => arbiter.DeregisterExpert(expert);

        private void Update() {
            // Execute all agreed actions from the current iteration
            foreach (var action in arbiter.BlackboardIteration(blackboard)) {
                try {
                    action();
                }
                catch (Exception e) {
                    Debug.LogError($"Action execution failed: {e}");
                    throw;
                }
            }
        }
    }
}