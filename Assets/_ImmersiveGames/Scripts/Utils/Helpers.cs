﻿namespace _ImmersiveGames.Scripts.Utils {
        public static class Helpers {
            public static void QuitGame() {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
}