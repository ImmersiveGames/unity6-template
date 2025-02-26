using System.Collections.Generic;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public interface IStatModifierApplicationOrder {
        int Apply(IEnumerable<StatModifier> statModifiers, int baseValue);
    }
}