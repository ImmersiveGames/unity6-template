using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Hotkeys.Editor {
    public static class LockInspector {
        private static readonly MethodInfo _flipLocked;
        private static readonly PropertyInfo _constrainProportions;
        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;

        static LockInspector() {
            // Cache static MethodInfo and PropertyInfo for performance
#if UNITY_2023_2_OR_NEWER
            var editorLockTrackerType = typeof(EditorGUIUtility).Assembly.GetType("UnityEditor.EditorGUIUtility+EditorLockTracker");
            _flipLocked = editorLockTrackerType.GetMethod("FlipLocked", BindingFlags);
#endif
            _constrainProportions = typeof(Transform).GetProperty("constrainProportionsScale", BindingFlags);
        }

        [MenuItem("Edit/Toggle Inspector Lock %l")]
        public static void Lock() {
#if UNITY_2023_2_OR_NEWER
            // New approach for Unity 2023.2 and above, including Unity 6
            var inspectorWindowType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");

            foreach (var inspectorWindow in Resources.FindObjectsOfTypeAll(inspectorWindowType)) {
                var lockTracker = inspectorWindowType.GetField("m_LockTracker", BindingFlags)
                    ?.GetValue(inspectorWindow);
                _flipLocked?.Invoke(lockTracker, new object[] { });
            }
#else
        // Old approach for Unity versions before 2023.2
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
#endif

            // Constrain Proportions lock for all versions including Unity 6
            foreach (var activeEditor in ActiveEditorTracker.sharedTracker.activeEditors) {
                if (activeEditor.target is not Transform target) continue;

                var currentValue = (bool) _constrainProportions.GetValue(target, null);
                _constrainProportions.SetValue(target, !currentValue, null);
            }

            ActiveEditorTracker.sharedTracker.ForceRebuild();
        }

        [MenuItem("Edit/Toggle Inspector Lock %l", true)]
        public static bool Valid() {
            return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
        }
    }
}
