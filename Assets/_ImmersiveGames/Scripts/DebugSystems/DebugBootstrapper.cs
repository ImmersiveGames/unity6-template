using _ImmersiveGames.Scripts.AbilitySystems;
using _ImmersiveGames.Scripts.AdvancedTimers;
using _ImmersiveGames.Scripts.BehaviorTreeSystem;
using _ImmersiveGames.Scripts.BusEventSystems;
using _ImmersiveGames.Scripts.EntitySystems;
using _ImmersiveGames.Scripts.GoapSystem;
using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using _ImmersiveGames.Scripts.StatesModifiers.Testes;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem;
using _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Tests;
using UnityEngine;
using Entity = _ImmersiveGames.Scripts.EntitySystems.Entity;

namespace _ImmersiveGames.Scripts.DebugSystems {
    public abstract class DebugBootstrapper {
        public static void Initialize()
        {
            DebugManager.SetGlobalDebugState(true);
            
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
            DebugManager.SetScriptDebugLevel<Entity>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Pickup>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnemyBaseState>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<BaseState>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<Injector>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<Provider>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<IEnvironmentSystem>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<SequenceNode>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EnterExitDecorator>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EventReactiveNode>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<CompositeConditionSo>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<CompareValueCondition>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<EntityBehavior>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<AbilityActionHandler>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<AbilityView>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<TimerTests>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<EventEmitterTest>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<GoapAgent>(DebugManager.DebugLevels.None);
            
            DebugManager.SetScriptDebugLevel<BehaviorTreeSo>(DebugManager.DebugLevels.None);
            DebugManager.SetScriptDebugLevel<NodeConfig>(DebugManager.DebugLevels.None);
            
            
            
            
            
            
            DebugManager.SetDefaultDebugLevel(DebugManager.DebugLevels.Logs);
            Debug.Log("Bootstrapper...");
        }
    }
}