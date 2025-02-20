using _ImmersiveGames.Scripts.Utils.ObserverSystems;

namespace _ImmersiveGames.Scripts.AbilitySystems {
    public class AbilityModel {
        public readonly ObservableList<Ability> Abilities = new();

        public void Add(Ability a) {
            Abilities.Add(a);
        }
    }
}