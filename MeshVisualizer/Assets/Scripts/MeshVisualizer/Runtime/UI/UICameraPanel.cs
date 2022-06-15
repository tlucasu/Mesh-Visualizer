using UnityEngine;
using UnityEngine.UIElements;


namespace MeshVisualizer.UI {
    public class UICameraPanel : UIPanel {
        private const string postProcessingContainerName = "post-processing-container";
        private const string toggleButtonClassName = "toggle-button";
        private const string toggleButtonEnabledClassName = "toggle-button-enabled";
        
        [SerializeField]
        private PostProcessingController postProcessingController;
        
        private VisualElement postProcessingContainer { get; set; }

        private void Start() {
            postProcessingContainer = contentContainer.Q<VisualElement>(postProcessingContainerName);
            
            InitializePostProcessingItems();
        }

        private void InitializePostProcessingItems() {
            var profile = postProcessingController.profile;
            postProcessingContainer.Clear();

            foreach (var component in profile.components) {
                Button toggleButton = new Button() {
                    name = $"{component.name}-button",
                    text = component.name
                };
                toggleButton.AddToClassList(toggleButtonClassName);
                
                if(component.active)
                    toggleButton.AddToClassList(toggleButtonEnabledClassName);

                toggleButton.RegisterCallback<ClickEvent>(PostProcessingItemClicked);
                postProcessingContainer.Add(toggleButton);
            }
        }

        private void PostProcessingItemClicked(ClickEvent evt) {
            var toggleButton = evt.target as Button;
            postProcessingController.ToggleComponent(toggleButton.text);
            toggleButton.ToggleInClassList(toggleButtonEnabledClassName);
        }
    }
}
