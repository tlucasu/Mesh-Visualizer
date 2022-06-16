using System.Linq;
using MeshVisualizer.Controller;
using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UICameraPanel : UIPanel {
        private const string cameraContainerName = "camera-container";
        private const string postProcessingContainerName = "post-processing-container";

        [Header("References")]
        [SerializeField]
        private CameraController cameraController;
        [SerializeField]
        private PostProcessingController postProcessingController;
        
        private VisualElement cameraContainer { get; set; }
        private VisualElement postProcessingContainer { get; set; }
        
        private Button activeCameraButton { get; set; }

        private void Start() {
            cameraContainer = contentContainer.Q<VisualElement>(cameraContainerName);
            InitializeCameraButtons();
            
            postProcessingContainer = contentContainer.Q<VisualElement>(postProcessingContainerName);
            InitializePostProcessingButtons();
        }

        private void InitializeCameraButtons() {
            cameraContainer.Clear();
            
            foreach (var camera in cameraController.cameras) {
                string elementName = camera.name.ToLower().Replace(" ", "-");
                Button button = new Button() {
                    name = $"{elementName}-button",
                    text = camera.gameObject.name
                };
                button.AddToClassList(panelButtonClassName);

                if (camera == cameraController.activeCamera) {
                    activeCameraButton = button;
                    button.AddToClassList(selectedPanelButtonClassName);
                }

                button.RegisterCallback<ClickEvent>(OnCameraButtonClicked);
                cameraContainer.Add(button);
            }
        }

        private void OnCameraButtonClicked(ClickEvent clickEvent) {
            var button = clickEvent.currentTarget as Button;
            if (button == activeCameraButton)
                return;
            
            if (activeCameraButton != null) {
                activeCameraButton.RemoveFromClassList(selectedPanelButtonClassName);
            }

            activeCameraButton = button;
            button.AddToClassList(selectedPanelButtonClassName);

            int index = cameraController.cameras.ToList().FindIndex(x => x.gameObject.name == button.text);
            cameraController.EnableCamera(index);
        }


        private void InitializePostProcessingButtons() {
            var profile = postProcessingController.profile;
            postProcessingContainer.Clear();

            foreach (var component in profile.components) {
                //Removes (Clone) from the name
                string name = component.name.Replace("(Clone)", "");
                
                Button toggleButton = new Button() {
                    name = $"{name}-button",
                    text = name
                };
                toggleButton.AddToClassList(panelButtonClassName);
                
                if(component.active)
                    toggleButton.AddToClassList(selectedPanelButtonClassName);

                toggleButton.RegisterCallback<ClickEvent>(OnPostProcessingButtonClicked);
                postProcessingContainer.Add(toggleButton);
            }
        }

        private void OnPostProcessingButtonClicked(ClickEvent clickEvent) {
            var toggleButton = clickEvent.currentTarget as Button;
            postProcessingController.ToggleComponent(toggleButton.text);
            toggleButton.ToggleInClassList(selectedPanelButtonClassName);
        }
    }
}
