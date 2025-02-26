using _ImmersiveGames.Scripts.DebugSystems;
using _ImmersiveGames.Scripts.Utils.MediatorSystems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _ImmersiveGames.Scripts.StatesModifiers.Testes {
    public abstract class Entity : MonoBehaviour, IVisitable {
        [SerializeField, InlineEditor, Required]
        private BaseStats baseStats;

        public Stats Stats { get; private set; }

        private void Awake() {
            Stats = new Stats(new StatsMediator(), baseStats);
        }

        public void Update() {
            Stats.Mediator.Update();
            DebugManager.Log<Entity>($"States: {Stats.Attack}, {Stats.Defense}");
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
}