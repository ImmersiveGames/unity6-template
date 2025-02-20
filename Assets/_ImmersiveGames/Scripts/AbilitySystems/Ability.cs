namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class Ability {
        public readonly AbilityData Data;

        public Ability(AbilityData data) {
            Data = data;
        }

        public AbilityCommand CreateCommand(int index, object source = null) {
            return new AbilityCommand(Data, index, source);
        }
    }
}