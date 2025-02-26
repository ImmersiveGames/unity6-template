namespace _ImmersiveGames.Scripts.StatesModifiers {
    public interface IStatModifierFactory {
        StatModifier Create(OperatorType operationType, StatType statType, int value, float duration);
    }
}