using UnityEditor;
using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Hotkeys.Editor {
    public static class CloseWindowTab {
        [MenuItem("File/Close Window Tab %w")]
        private static void CloseTab() {
            var focusedWindow = EditorWindow.focusedWindow;
            if (focusedWindow != null) {
                CloseTab(focusedWindow);
            } else {
                Debug.LogWarning("Found no focused window to close");
            }
        }

        private static void CloseTab(EditorWindow editorWindow) {
            editorWindow.Close();
        }
    }
}