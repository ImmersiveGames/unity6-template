using _ImmersiveGames.Scripts.StatesModifiers.Interfaces;
using _ImmersiveGames.Scripts.StatesModifiers.Testes;
using _ImmersiveGames.Scripts.Utils.ServiceLocatorSystems;
using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public enum OperatorType {
        Add,
        Multiply
    }

    [RequireComponent(typeof(AudioSource))]
    public class StatModifierPickup : Pickup {
        // TODO Move configuration to ScriptableObject
        [SerializeField] StatType type = StatType.Attack;
        [SerializeField] OperatorType operatorType = OperatorType.Add;
        [SerializeField] int value = 10;
        [SerializeField] float duration = 5f;

        IStatModifierFactory statModifierFactory;

        protected override void ApplyPickupEffect(Entity entity) {
            var modifier = ServiceLocator.For(this).Get<IStatModifierFactory>().Create(operatorType, type, value, duration);
            entity.Stats.Mediator.AddModifier(modifier);
        }
    }
}