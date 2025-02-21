namespace _ImmersiveGames.Scripts.StatesModifiers.Interfaces {
    public interface IStatModifierFactory {
        StatModifier Create(OperatorType operationType, StatType statType, int value, float duration);
    }
}