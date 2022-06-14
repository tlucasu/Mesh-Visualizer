using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public abstract class UIPanel : MonoBehaviour {
        [Header("UI Properties")]
        [SerializeField]
        protected VisualTreeAsset panelUxml;
        
        [SerializeField]
        [Tooltip("Name of the tab button in the UXML that when clicked will toggle the panel to display this content")]
        protected string tabName;

        protected string contentContainerName => tabName + "-content-container";

        protected VisualElement contentContainer { get; set; }

        protected void Awake() {
            if (panelUxml == null) {
                Debug.LogError($"Failed to initialize {this}, 'panelUxml' is null");
                return;
            }
            
            var document = GetComponentInParent<UIDocument>();
            if (document == null) {
                Debug.LogError($"Failed to initialize {this}, parent must contain UIDocument component");
                return;
            }

            contentContainer = document.rootVisualElement.Q<VisualElement>(contentContainerName);
            if (contentContainer == null) {
                Debug.LogError($"Failed to initialize {this}. Could not find container with the name " +
                               $"'{contentContainerName}'. Check Tab Name member.");
                return;
            }
            
            //Add style sheets
            foreach(var stylesheet in panelUxml.stylesheets)
                document.rootVisualElement.styleSheets.Add(stylesheet);
            
            contentContainer.Add(panelUxml.Instantiate());
            contentContainer.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
        }
    }
}
