using UnityEngine;
using UnityEngine.InputSystem;

namespace MeshVisualizer.Input {
    public class ScreenInputManager : MonoBehaviour {
        private InputControls inputControls { get; set; }
        private void Awake() {
            inputControls = new InputControls();
            inputControls.Default.ScreenPress.started += OnScreenPressed;
            inputControls.Default.ScreenPress.canceled += OnScreenReleased;
        }

        private void OnDestroy() {
            inputControls.Default.ScreenPress.started -= OnScreenPressed;
            inputControls.Default.ScreenPress.canceled -= OnScreenReleased;
        }

        private void OnEnable() {
            inputControls.Enable();
        }

        private void OnDisable() {
            inputControls.Disable();
        }
        private void OnScreenPressed(InputAction.CallbackContext context) {
            inputControls.Default.ScreenDrag.performed += OnScreenDrag;
        }
        
        private void OnScreenReleased(InputAction.CallbackContext context) {
            inputControls.Default.ScreenDrag.performed -= OnScreenDrag;
        }

        private void OnScreenDrag(InputAction.CallbackContext context) {
            var value = context.ReadValue<Vector2>();
            Debug.Log(value);
        }
    }
}