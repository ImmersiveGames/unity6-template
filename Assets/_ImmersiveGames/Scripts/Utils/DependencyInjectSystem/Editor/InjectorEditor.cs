using UnityEditor;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.DependencyInjectSystem.Editor {
    [CustomEditor(typeof(Injector))]
    public class InjectorEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            var injector = (Injector) target;

            if (GUILayout.Button("Validate Dependencies")) {
                injector.ValidateDependencies();
            }

            if (!GUILayout.Button("Clear All Injectable Fields")) return;
            injector.ClearDependencies();
            EditorUtility.SetDirty(injector);
        }
    }
}