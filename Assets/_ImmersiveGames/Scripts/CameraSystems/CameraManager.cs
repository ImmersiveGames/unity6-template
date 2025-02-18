using System.Collections;
using _ImmersiveGames.Scripts.InputSystems;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace _ImmersiveGames.Scripts.CameraSystems {
    public class CameraManager : ValidatedMonoBehaviour {
        [Header("References")]
        [SerializeField, Anywhere]
        private InputReader input;
        [SerializeField, Anywhere] private CinemachineFreeLook freeLookVCam;
        
        [Header("Settings")] 
        [SerializeField, Range(0.5f, 3f)]
        private float speedMultiplier = 1f;

        private bool _isRmbPressed;
        private bool _cameraMovementLock;

        private void OnEnable() {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable() {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
            
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse) {
            if (_cameraMovementLock) return;
            
            if (isDeviceMouse && !_isRmbPressed) return;

            // If the device is mouse use fixedDeltaTime, otherwise use deltaTime
            var deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
            // Set the camera axis values
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
        }

        private void OnEnableMouseControlCamera() {
            _isRmbPressed = true;
            
            // Lock the cursor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            StartCoroutine(DisableMouseForFrame());
        }

        private void OnDisableMouseControlCamera() {
            _isRmbPressed = false;
            
            // Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Reset the camera axis to prevent jumping when re-enabling mouse control
            freeLookVCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0f;
        }

        private IEnumerator DisableMouseForFrame() {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }
    }
}