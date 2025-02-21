using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Interface;
using _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.GoapSystem {

    public class GoapFactory : MonoBehaviour, IDependencyProvider {
        void Awake() {
            ServiceLocator.Global.Register(this);
        }
    
        [Provide] public GoapFactory ProvideFactory() => this;

        public IGoapPlanner CreatePlanner() {
            return new GoapPlanner();
        }
    }
}