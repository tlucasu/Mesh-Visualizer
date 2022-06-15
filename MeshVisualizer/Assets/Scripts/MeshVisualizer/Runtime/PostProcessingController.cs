using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingController : MonoBehaviour {
    public VolumeProfile profile;

    public void ToggleComponent(string componentName) {
        var component = profile.components.Find(x => x.name == componentName);
        if (component == null) {
            return;
        }

        component.active = !component.active;
    }
}
