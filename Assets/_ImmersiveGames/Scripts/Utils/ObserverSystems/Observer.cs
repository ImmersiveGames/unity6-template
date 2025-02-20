﻿using System;
using System.Reflection;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

namespace _ImmersiveGames.Scripts.Utils.ObserverSystems {
    [Serializable]
    public class Observer<T> {
        [SerializeField] T value;
        [SerializeField] UnityEvent<T> onValueChanged;

        public T Value {
            get => value;
            set => Set(value);
        }
    
        public static implicit operator T(Observer<T> observer) => observer.value;
    
        public Observer(T value, UnityAction<T> callback = null) {
            this.value = value;
            onValueChanged = new UnityEvent<T>();
            if (callback != null) onValueChanged.AddListener(callback);
        }

        public void Set(T setValue) {
            if (Equals(this.value, setValue)) return;
            value = setValue;
            Invoke();
        }

        public void Invoke() {
            Debug.Log($"Invoking {onValueChanged.GetPersistentEventCount()} listeners");
            onValueChanged.Invoke(value);
        }

        public void AddListener(UnityAction<T> callback) {
            if (callback == null) return;
            onValueChanged ??= new UnityEvent<T>();

#if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(onValueChanged, callback);
#else
        onValueChanged.AddListener(callback);
#endif
        }
    
        public void RemoveListener(UnityAction<T> callback) {
            if (callback == null) return;
            if (onValueChanged == null) return;
        
#if UNITY_EDITOR
            UnityEventTools.RemovePersistentListener(onValueChanged, callback);
#else
        onValueChanged.RemoveListener(callback);
#endif
        }
    
        public void RemoveAllListeners() {
            if (onValueChanged == null) return;
        
#if UNITY_EDITOR
            var fieldInfo = typeof(UnityEventBase).GetField("m_PersistentCalls", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null) return;
            var fieldValue = fieldInfo.GetValue(onValueChanged);
            fieldValue.GetType().GetMethod("Clear")?.Invoke(fieldValue, null);

#else        
        onValueChanged.RemoveAllListeners();
#endif
        }
    
        public void Dispose() {
            RemoveAllListeners();
            onValueChanged = null;
            value = default;
        }
    }
}