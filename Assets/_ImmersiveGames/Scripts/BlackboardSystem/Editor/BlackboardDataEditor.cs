using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace _ImmersiveGames.Scripts.BlackboardSystem.Editor {

    [CustomEditor(typeof(BlackboardData))]
    public class BlackboardDataEditor : UnityEditor.Editor {
        ReorderableList entryList;
        private static readonly Dictionary<AnyValue.ValueType, Action<Rect, SerializedProperty>> valueDrawers = new() {
            { AnyValue.ValueType.Int, (rect, value) => EditorGUI.PropertyField(rect, value.FindPropertyRelative("intValue"), GUIContent.none) },
            { AnyValue.ValueType.Float, (rect, value) => EditorGUI.PropertyField(rect, value.FindPropertyRelative("floatValue"), GUIContent.none) },
            { AnyValue.ValueType.Bool, (rect, value) => EditorGUI.PropertyField(rect, value.FindPropertyRelative("boolValue"), GUIContent.none) },
            { AnyValue.ValueType.String, (rect, value) => EditorGUI.PropertyField(rect, value.FindPropertyRelative("stringValue"), GUIContent.none) },
            { AnyValue.ValueType.Vector3, (rect, value) => EditorGUI.PropertyField(rect, value.FindPropertyRelative("vector3Value"), GUIContent.none) }
        };
        void OnEnable() {
            entryList = new ReorderableList(serializedObject, serializedObject.FindProperty("entries"), true, true, true, true) {
                drawHeaderCallback = rect => {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Key");
                    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.3f + 10, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Type");
                    EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.6f + 5, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight), "Value");
                }
            };

            entryList.drawElementCallback = (rect, index, _, _) => {
                var element = entryList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                var keyName = element.FindPropertyRelative("keyName");
                var valueType = element.FindPropertyRelative("valueType");
                var value = element.FindPropertyRelative("value");
    
                var keyNameRect = new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                var valueTypeRect = new Rect(rect.x + rect.width * 0.3f, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
                var valueRect = new Rect(rect.x + rect.width * 0.6f, rect.y, rect.width * 0.4f, EditorGUIUtility.singleLineHeight);
    
                EditorGUI.PropertyField(keyNameRect, keyName, GUIContent.none);
                EditorGUI.PropertyField(valueTypeRect, valueType, GUIContent.none);
                valueDrawers[(AnyValue.ValueType)valueType.enumValueIndex](valueRect, value);
            };
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            entryList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
