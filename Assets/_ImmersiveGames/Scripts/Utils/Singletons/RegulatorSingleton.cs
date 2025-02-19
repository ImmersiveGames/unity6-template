using UnityEngine;

namespace _ImmersiveGames.Scripts.Utils.Singletons {
    /// <summary>
    /// Persistent Regulator singleton, will destroy any other older components of the same type it finds on awake
    /// </summary>
    public class RegulatorSingleton<T> : MonoBehaviour where T : Component {
        private static T _instance;

        public static bool HasInstance => _instance != null;

        public float InitializationTime { get; private set; }

        public static T Instance {
            get {
                if (_instance != null) return _instance;
                _instance = FindAnyObjectByType<T>();
                if (_instance != null) return _instance;
                var go = new GameObject(typeof(T).Name + " Auto-Generated") {
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = go.AddComponent<T>();

                return _instance;
            }
        }

        /// <summary>
        /// Make sure to call base.Awake() in override if you need awake.
        /// </summary>
        private void Awake() {
            InitializeSingleton();
        }

        private void InitializeSingleton() {
            if (!Application.isPlaying) return;
            InitializationTime = Time.time;
            DontDestroyOnLoad(gameObject);

            var oldInstances = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (var old in oldInstances) {
                if (old.GetComponent<RegulatorSingleton<T>>().InitializationTime < InitializationTime) {
                    Destroy(old.gameObject);
                }
            }

            if (_instance == null) {
                _instance = this as T;
            }
        }
    }
}