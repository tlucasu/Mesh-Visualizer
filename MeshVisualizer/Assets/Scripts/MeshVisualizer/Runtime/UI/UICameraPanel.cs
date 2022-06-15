using UnityEngine;
using UnityEngine.UIElements;


namespace MeshVisualizer.UI {
    public class UICameraPanel : UIPanel {
        private const string postProcessingContainerName = "post-processing-container";
        [SerializeField]
        private PostProcessingController postProcessingController;
        
        private VisualElement postProcessingContainer { get; set; }

        private void Start() {
            postProcessingContainer = contentContainer.Q<VisualElement>(postProcessingContainerName);
            
            InitializePostProcessingItems();
        }

        private void InitializePostProcessingItems() {
            var profile = postProcessingController.profile;

            foreach (var component in profile.components) {
                Button itemButton = new Button() {
                    name = $"{component.name}-button",
                    text = component.displayName
                };
                // itemButton.AddToClassList(objectItemClass);

                itemButton.RegisterCallback<ClickEvent>(PostProcessingItemClicked);
                postProcessingContainer.Add(itemButton);
            }
        }

        private void PostProcessingItemClicked(ClickEvent evt) {
            var itemButton = evt.target as Button;
            postProcessingController.ToggleComponent(itemButton.text);
        }
    }
}
