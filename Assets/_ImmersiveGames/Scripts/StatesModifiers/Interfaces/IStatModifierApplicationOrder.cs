using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.StatesModifiers.Interfaces {
    public interface IStatModifierApplicationOrder {
        int Apply(IEnumerable<StatModifier> statModifiers, int baseValue);
    }
}