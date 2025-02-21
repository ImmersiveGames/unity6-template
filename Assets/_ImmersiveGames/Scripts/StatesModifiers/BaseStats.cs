using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    [CreateAssetMenu(fileName = "BaseStats", menuName = "ImmersiveGames/Stats/BaseStats")]
    public class BaseStats : ScriptableObject {
        public int attack = 10;
        public int defense = 20;
    }
}