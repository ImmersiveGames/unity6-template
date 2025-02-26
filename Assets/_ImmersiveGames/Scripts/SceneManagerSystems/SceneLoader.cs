using System;
using System.Threading.Tasks;
using _ImmersiveGames.Scripts.SceneManagerSystems.Systems.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _ImmersiveGames.Scripts.SceneManagerSystems {
    public class SceneLoader : MonoBehaviour { 
        [SerializeField] private Image loadingBar;
        [SerializeField] private float fillSpeed = 0.5f;
        [SerializeField] private Canvas loadingCanvas;
        [SerializeField] private Camera loadingCamera;
        [SerializeField] private SceneGroup[] sceneGroups;

        private float targetProgress;
        private bool isLoading;

        public readonly SceneGroupManager Manager = new SceneGroupManager();

        private void Awake() {
            // TODO can remove
            Manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            Manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            Manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }

        private async void Start() {
            await LoadSceneGroup(0);
        }

        private void Update() {
            if (!isLoading) return;
            
            var currentFillAmount = loadingBar.fillAmount;
            var progressDifference = Mathf.Abs(currentFillAmount - targetProgress);

            var dynamicFillSpeed = progressDifference * fillSpeed;
    
            loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime * dynamicFillSpeed);
        }

        public async Task LoadSceneGroup(int index) {
            loadingBar.fillAmount = 0f;
            targetProgress = 1f;

            if (index < 0 || index >= sceneGroups.Length) {
                Debug.LogError("Invalid scene group index: " + index);
                return;
            }

            var progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);
            
            EnableLoadingCanvas();
            await Manager.LoadScenes(sceneGroups[index], progress);
            EnableLoadingCanvas(false);
        }

        private void EnableLoadingCanvas(bool enable = true) {
            isLoading = enable;
            loadingCanvas.gameObject.SetActive(enable);
            loadingCamera.gameObject.SetActive(enable);
        }
        
    }
}