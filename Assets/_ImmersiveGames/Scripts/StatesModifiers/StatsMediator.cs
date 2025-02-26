using System.Collections.Generic;
using System.Linq;

namespace _ImmersiveGames.Scripts.StatesModifiers {
    public class StatsMediator {
        private readonly List<StatModifier> listModifiers = new();
        private readonly Dictionary<StatType, IEnumerable<StatModifier>> modifierCache = new();
        private readonly IStatModifierApplicationOrder order = new NormalStatModifierOrder(); // OR Inject

        // NOTE: Delegates are invoked in the order they are added to the event.
        public void PerformQuery(object sender, Query query) {
            if (!modifierCache.ContainsKey(query.StatType)) {
                modifierCache[query.StatType] = listModifiers.Where(modifier=> modifier.Type == query.StatType).ToList();
            }
            query.Value = order.Apply(modifierCache[query.StatType], query.Value);
        }

        private void InvalidateCache(StatType statType) {
            modifierCache.Remove(statType);
        }

        public void AddModifier(StatModifier modifier) {
            listModifiers.Add(modifier);
            InvalidateCache(modifier.Type);
            modifier.MarkedForRemoval = false;
            modifier.OnDispose += _ => InvalidateCache(modifier.Type);
            modifier.OnDispose += _ => listModifiers.Remove(modifier);
        }

        public void Update() {
            /*
             //Refactor: Se precisar de update para os modificadores executar aqui
             foreach (var modifier in listModifiers) {
                modifier.Update();
            }
             */
            foreach (var modifier in listModifiers.Where(modifier => modifier.MarkedForRemoval).ToList()) {
                modifier.Dispose();
            }
        }
    }

    public class Query {
        public readonly StatType StatType;
        public int Value;

        public Query(StatType statType, int value) {
            StatType = statType;
            Value = value;
        }
    }
}