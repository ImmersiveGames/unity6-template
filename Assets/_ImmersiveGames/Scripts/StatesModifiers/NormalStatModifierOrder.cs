using System.Collections.Generic;
using System.Linq;
using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;
using _ImmersiveGames.Scripts.StatesModifiers.Strategies;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class NormalStatModifierOrder : IStatModifierApplicationOrder {
        public int Apply(IEnumerable<StatModifier> statModifiers, int baseValue) {
            var allModifiers = statModifiers.ToList();

            foreach (var modifier in allModifiers.Where(modifier => modifier.Strategy is AddOperation)) {
                baseValue = modifier.Strategy.Calculate(baseValue);
            }

            foreach (var modifier in allModifiers.Where(modifier => modifier.Strategy is MultiplyOperation)) {
                baseValue = modifier.Strategy.Calculate(baseValue);
            }

            return baseValue;
        }
    }
}