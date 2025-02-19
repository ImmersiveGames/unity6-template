using _ImmersiveGames.Scripts.EntitySystems;
using _ImmersiveGames.Scripts.EntitySystems.EnemyStates;
using _ImmersiveGames.Scripts.EntitySystems.PlayerStates;
using _ImmersiveGames.Scripts.GameSystems.BusEvents;
using _ImmersiveGames.Scripts.Utils.DebugSystems;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests;
using _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.GameSystems.Debugs {
    [DefaultExecutionOrder(-100)]
    public class DebugTest : MonoBehaviour {
        private void Awake() {
            #region Util - Debugs

            DebugManager.SetScriptDebugLevel<FrameRateLimiter>(DebugManager.DebugLevels.None);
            
            #endregion
            #region Debug Service Locator
            DebugManager.SetScriptDebugLevel<TestServiceTwo>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<TestServices>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<ServiceManager>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<ServiceLocator>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<MockLocalization>(DebugManager.DebugLevels.None);
            
            #endregion
            
            DebugManager.SetScriptDebugLevel<TestBusBinding>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<PlayerController>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<Health>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemyBaseState>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BaseState>(DebugManager.DebugLevels.None);
            
            
            DebugManager.SetScriptDebugLevel<Injector>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Provider>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<IEnvironmentSystem>(DebugManager.DebugLevels.None);
            
            
            
            
                
        }
    }
}