﻿namespace _ImmersiveGames.Scripts.Utils.Helpers {
        public static class ApplicationHelper {
            public static void QuitGame() {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
}