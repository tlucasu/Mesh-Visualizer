using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MeshVisualizer.Input {
    public class ScreenInputManager : MonoBehaviour {
        public static ScreenInputManager Instance { get; set; }
        public ScreenInputManager() {
            Instance = this;
        }
        
        private InputControls inputControls { get; set; }
        private bool controlsEnabled { get; set; }

        [Tooltip("Called when either touch or mouse pointer is dragged on the screen")]
        public UnityEvent<Vector2> onPointerDrag;
        
        public UnityEvent onPointerPressed;
        public UnityEvent onPointerReleased;
        
        public Vector2 pointerScreenPosition { get; set; }
        
        private void Awake() {
            inputControls = new InputControls();
            inputControls.Default.ScreenPress.started += OnScreenPressed;
            inputControls.Default.ScreenPress.canceled += OnScreenReleased;
            controlsEnabled = true;
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

        public static void Disable() {
            if (Instance.controlsEnabled) {
                Instance.inputControls.Disable();
                Instance.controlsEnabled = false;
            }
        }

        public static void Enable() {
            if (!Instance.controlsEnabled) {
                Instance.inputControls.Enable();
                Instance.controlsEnabled = true;
            }
        }

        private void OnScreenPressed(InputAction.CallbackContext context) {
            //initialize the screen position before starting to read
            pointerScreenPosition = inputControls.Default.ScreenDrag.ReadValue<Vector2>();
            inputControls.Default.ScreenDrag.performed += OnScreenDrag;
            onPointerPressed.Invoke();
        }
        
        private void OnScreenReleased(InputAction.CallbackContext context) {
            inputControls.Default.ScreenDrag.performed -= OnScreenDrag;
            // inputControls.Default.ScreenPosition
            onPointerReleased.Invoke();
        }

        private void OnScreenDrag(InputAction.CallbackContext context) {
            var newScreenPosition = context.ReadValue<Vector2>();
            
            var delta = newScreenPosition - pointerScreenPosition;
            pointerScreenPosition = newScreenPosition;
            
            onPointerDrag.Invoke(delta);
        }
    }
}