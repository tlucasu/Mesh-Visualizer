using UnityEngine;

namespace MeshVisualizer.Controller {
    public class CameraController : MonoBehaviour {

        public Camera activeCamera { get; private set; }

        public Camera[] cameras;

        private void Reset() {
            cameras = GetComponentsInChildren<Camera>();
        }

        private void Awake() {
            foreach (var cam in cameras) {
                if (cam.isActiveAndEnabled) {
                    if (activeCamera == null) {
                        activeCamera = cam;
                    }
                    else {
                        //Ensure all other cameras are disabled
                        cam.gameObject.SetActive(false);
                    }
                }
            }

            if (activeCamera == null) {
                Debug.LogWarning($"{this} cannot find an active camera. Enabling first camera");
                EnableCamera(0);
            }
        }

        public void EnableCamera(int index) {
            if (index < 0 || index >= cameras.Length) {
                Debug.LogWarning($"Index is out of range. Enabling first camera");
                index = 0;
            }

            for (int i = 0; i < cameras.Length; i++) {
                if (i == index) {
                    activeCamera = cameras[i];
                    cameras[i].gameObject.SetActive(true);
                }
                else {
                    cameras[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
