using System;
using _ImmersiveGames.Scripts.AdvancedTimers.Timers;
using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;
using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class StatModifier : IDisposable {
        public StatType Type { get; }
        public IOperationStrategy Strategy { get; }
    
        public readonly Sprite Icon;
        public bool MarkedForRemoval { get; set; } // TODO: Make private and add a public method to set it
    
        public event Action<StatModifier> OnDispose = delegate { };

        public StatModifier(StatType type, IOperationStrategy strategy, float duration) {
            Type = type;
            Strategy = strategy;
            if (duration <= 0) return;
        
            var timer1 = new CountdownTimer(duration);
            timer1.OnTimerStop += () => MarkedForRemoval = true;
            timer1.Start();
        }
    
        public void Handle(object sender, Query query) {
            if (query.StatType == Type) {
                query.Value = Strategy.Calculate(query.Value);
            }
        }

        public void Dispose() {
            OnDispose.Invoke(this);
        }
    }
}