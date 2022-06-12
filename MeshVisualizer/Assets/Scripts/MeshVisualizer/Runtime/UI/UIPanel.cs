using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public abstract class UIPanel : MonoBehaviour {
        [SerializeField]
        [Tooltip("Name of the tab button in the UXML that when clicked will toggle the panel to display this content")]
        protected string tabName;

        protected string contentContainerName => tabName + "-content-container";

        protected VisualElement contentContainer { get; set; }

        protected void Awake() {
            var document = GetComponentInParent<UIDocument>();
            if (document == null) {
                Debug.LogError($"Failed to initialize {this}, parent must contain UIDocument component");
                return;
            }

            contentContainer = document.rootVisualElement.Q<VisualElement>(contentContainerName);
            if (contentContainer == null) {
                Debug.LogError($"Failed to initailize {this}. Could not find container with the name " +
                               $"'{contentContainerName}'. Check Tab Name member.");
            }
        }
    }
}
