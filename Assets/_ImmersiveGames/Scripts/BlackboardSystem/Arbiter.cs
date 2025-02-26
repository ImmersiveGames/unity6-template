using System;
using System.Collections.Generic;
using _ImmersiveGames.Scripts.BlackboardSystem.BlackboardSystem;
using _ImmersiveGames.Scripts.Utils;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    public class Arbiter {
        private readonly List<IExpert> experts = new();

        public void RegisterExpert(IExpert expert) {
            Preconditions.CheckNotNull(expert);
            experts.Add(expert);
        }

        public void DeregisterExpert(IExpert expert) {
            Preconditions.CheckNotNull(expert);
            experts.Remove(expert);
        }

        public List<Action> BlackboardIteration(Blackboard blackboard) {
            IExpert bestExpert = null;
            var highestInsistence = 0;

            foreach (var expert in experts) {
                try {
                    var insistence = expert.GetInsistence(blackboard);
                    if (insistence <= highestInsistence) continue;
                    highestInsistence = insistence;
                    bestExpert = expert;
                }
                catch (Exception e) {
                    Debug.LogError($"Action execution failed: {e}");
                    throw;
                }
            }
            
            bestExpert?.Execute(blackboard);
            
            var actions = new List<Action>(blackboard.PassedActions);
            blackboard.ClearActions();
            
            // Return or execute the actions here
            return actions;
        }
    }
}