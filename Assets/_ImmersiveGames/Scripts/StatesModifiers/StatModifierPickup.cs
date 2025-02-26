using _ImmersiveGames.Scripts.ServiceLocatorSystems;
using _ImmersiveGames.Scripts.StatesModifiers.Testes;
using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public enum OperatorType {
        Add,
        Multiply
    }

    [RequireComponent(typeof(AudioSource))]
    public class StatModifierPickup : Pickup {
        // TODO Move configuration to ScriptableObject
        [SerializeField] private StatType type = StatType.Attack;
        [SerializeField] private OperatorType operatorType = OperatorType.Add;
        [SerializeField] private int value = 10;
        [SerializeField] private float duration = 5f;

        private IStatModifierFactory statModifierFactory;

        protected override void ApplyPickupEffect(Entity entity) {
            var modifier = ServiceLocator.For(this).Get<IStatModifierFactory>().Create(operatorType, type, value, duration);
            entity.Stats.Mediator.AddModifier(modifier);
        }
    }
}