using System;
using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;
using _ImmersiveGames.Scripts.StatesModifiers.Strategies;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class StatModifierFactory : IStatModifierFactory {
        public StatModifier Create(OperatorType operatorType, StatType statType, int value, float duration) {
            IOperationStrategy strategy = operatorType switch {
                OperatorType.Add => new AddOperation(value),
                OperatorType.Multiply => new MultiplyOperation(value),
                _ => throw new ArgumentOutOfRangeException()
            };
        
            return new StatModifier(statType, strategy, duration);
        }
    }
}