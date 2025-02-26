using System;
using System.Collections.Generic;
using _ImmersiveGames.Scripts.BlackboardSystem.BlackboardSystem;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BlackboardSystem {
    [CreateAssetMenu(fileName = "New Blackboard Data", menuName = "Blackboard/Blackboard Data")]
    public class BlackboardData : ScriptableObject {
        public List<BlackboardEntryData> entries = new();

        public void SetValuesOnBlackboard(Blackboard blackboard) {
            foreach (var entry in entries) {
                entry.SetValueOnBlackboard(blackboard);
            }
        }
    }

    [Serializable]
    public class BlackboardEntryData : ISerializationCallbackReceiver {
        public string keyName;
        public AnyValue.ValueType valueType;
        public AnyValue value;

        public void SetValueOnBlackboard(Blackboard blackboard) {
            var key = blackboard.GetOrRegisterKey(keyName);
            _setValueDispatchTable[value.type](blackboard, key, value);
        }
        
        // Dispatch table to set different types of value on the blackboard
        private static readonly Dictionary<AnyValue.ValueType, Action<Blackboard, BlackboardKey, AnyValue>> _setValueDispatchTable = new() {
            { AnyValue.ValueType.Int, SetValue<int> },
            { AnyValue.ValueType.Float, SetValue<float> },
            { AnyValue.ValueType.Bool, SetValue<bool> },
            { AnyValue.ValueType.String, SetValue<string> },
            { AnyValue.ValueType.Vector3, SetValue<Vector3> },
        };
        private static void SetValue<T>(Blackboard blackboard, BlackboardKey key, AnyValue anyValue) {
            blackboard.SetValue(key, anyValue.ConvertValue<T>());
        }
        
        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => value.type = valueType;
    }

    [Serializable]
    public struct AnyValue {
        public enum ValueType { Int, Float, Bool, String, Vector3 }
        public ValueType type;
        
        // Storage for different types of values
        public int intValue;
        public float floatValue;
        public bool boolValue;
        public string stringValue;
        public Vector3 vector3Value;
        // Add more types as needed, but remember to add them to the dispatch table above and the custom Editor
        
        // Implicit conversion operators to convert AnyValue to different types
        public static implicit operator int(AnyValue value) => value.ConvertValue<int>();
        public static implicit operator float(AnyValue value) => value.ConvertValue<float>();
        public static implicit operator bool(AnyValue value) => value.ConvertValue<bool>();
        public static implicit operator string(AnyValue value) => value.ConvertValue<string>();
        public static implicit operator Vector3(AnyValue value) => value.ConvertValue<Vector3>();

        public T ConvertValue<T>() {
            T result = type switch {
                ValueType.Int => AsInt<T>(intValue),
                ValueType.Float => AsFloat<T>(floatValue),
                ValueType.Bool => AsBool<T>(boolValue),
                ValueType.String => (T)(object)stringValue,
                ValueType.Vector3 => AsVector3<T>(vector3Value),
                _ => throw new NotSupportedException($"Not supported value type: {typeof(T)}")
            };
            if (EqualityComparer<T>.Default.Equals(result, default))
                Debug.LogWarning($"Type mismatch converting {type} to {typeof(T)}");
            return result;
        }
        
        // Helper methods for safe type conversions of the value types without the cost of boxing
        T AsBool<T>(bool value) => typeof(T) == typeof(bool) && value is T correctType ? correctType : default;
        T AsInt<T>(int value) => typeof(T) == typeof(int) && value is T correctType ? correctType : default;
        T AsFloat<T>(float value) => typeof(T) == typeof(float) && value is T correctType ? correctType : default;
        T AsVector3<T>(Vector3 value) => typeof(T) == typeof(Vector3) && value is T correctType ? correctType : default;
    }
}