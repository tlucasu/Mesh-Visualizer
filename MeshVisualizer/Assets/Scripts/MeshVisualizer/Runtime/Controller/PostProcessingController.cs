using UnityEngine;
using UnityEngine.Rendering;

namespace MeshVisualizer.Controller {
    [RequireComponent(typeof(Volume))]
    public class PostProcessingController : MonoBehaviour {
        public Volume volume { get; private set; }
        public VolumeProfile profile => volume.profile;

        private void Awake() {
            volume = GetComponent<Volume>();
        }

        public void ToggleComponent(string componentName) {
            var component = profile.components.Find(x => x.name.Contains(componentName));
            if (component == null) {
                return;
            }

            component.active = !component.active;
        }
    }
}
