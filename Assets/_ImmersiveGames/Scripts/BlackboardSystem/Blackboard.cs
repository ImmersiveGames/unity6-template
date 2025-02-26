using System;
using System.Collections.Generic;
using _ImmersiveGames.Scripts.Utils;
using _ImmersiveGames.Scripts.Utils.Extensions;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    namespace BlackboardSystem {
        [Serializable]
        public readonly struct BlackboardKey : IEquatable<BlackboardKey> {
            private readonly string name;
            private readonly int hashedKey;

            public BlackboardKey(string name) {
                this.name = name;
                hashedKey = name.ComputeFnv1AHash();
            }

            public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;

            public override bool Equals(object obj) => obj is BlackboardKey other && Equals(other);
            public override int GetHashCode() => hashedKey;
            public override string ToString() => name;

            public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
            public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => !(lhs == rhs);
        }

        [Serializable]
        public class BlackboardEntry<T> {
            public BlackboardKey Key { get; }
            public T Value { get; }
            public Type ValueType { get; }

            public BlackboardEntry(BlackboardKey key, T value) {
                Key = key;
                Value = value;
                ValueType = typeof(T);
            }

            public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
            public override int GetHashCode() => Key.GetHashCode();
        }

        [Serializable]
        public class Blackboard {
            Dictionary<string, BlackboardKey> keyRegistry = new();
            Dictionary<BlackboardKey, object> entries = new();

            public List<Action> PassedActions { get; } = new();

            public void AddAction(Action action) {
                Preconditions.CheckNotNull(action);
                PassedActions.Add(action);
            }

            public void ClearActions() => PassedActions.Clear();

            public void Debug() {
                foreach (var entry in entries) {
                    if (entry.Value is BlackboardEntry<object> genericEntry) {
                        UnityEngine.Debug.Log($"Key: {entry.Key}, Value: {genericEntry.Value}");
                    }
                }
            }

            public bool TryGetValue<T>(BlackboardKey key, out T value) {
                if (entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry) {
                    value = castedEntry.Value;
                    return true;
                }

                value = default;
                return false;
            }

            public void SetValue<T>(BlackboardKey key, T value) {
                entries[key] = new BlackboardEntry<T>(key, value);
            }

            public BlackboardKey GetOrRegisterKey(string keyName) {
                Preconditions.CheckNotNull(keyName);

                if (keyRegistry.TryGetValue(keyName, out BlackboardKey key)) return key;
                key = new BlackboardKey(keyName);
                keyRegistry[keyName] = key;

                return key;
            }

            public bool ContainsKey(BlackboardKey key) => entries.ContainsKey(key);

            public void Remove(BlackboardKey key) => entries.Remove(key);
        }
    }
}