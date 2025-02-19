using _ImmersiveGames.Scripts.Utils.DebugSystems;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Interface;
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