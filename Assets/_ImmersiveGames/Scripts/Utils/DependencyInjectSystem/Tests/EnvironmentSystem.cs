using _ImmersiveGames.Scripts.DebugSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests {
    public class EnvironmentSystem : MonoBehaviour, IEnvironmentSystem, IDependencyProvider {
        [Provide]
        private IEnvironmentSystem ProvideEnvironmentSystem() {
            return this;
        }

        public void Initialize() {
            DebugManager.Log<IEnvironmentSystem>("ProvideEnvironmentSystem().Initialize()");
        }
    }
}