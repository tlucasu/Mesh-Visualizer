using MeshVisualizer.Controller;
using UnityEngine;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UILightPanel : UIPanel {
        private const string defaultSkyboxButtonName = "default-skybox-button";
        private const string darkSkyboxButtonName = "dark-skybox-button";
        private const string noSkyboxButtonName = "no-skybox-button";

        private const string additionalLightContainerName = "additional-light-container";

        [Header("Reference")]
        [SerializeField]
        private LightingController lightingController;
        
        private Button activeSkyboxButton { get; set; }
        
        void Start() {
            InitializeSkyboxButtons();
            InitializeAdditionalLightButtons();
        }

        private void InitializeAdditionalLightButtons() {
            var lightContainer = contentContainer.Q<VisualElement>(additionalLightContainerName);
            lightContainer.Clear();

            foreach (var lightGameObject in lightingController.additionalLights) {
                if (lightGameObject == null) 
                    continue;
                
                string elementName = lightGameObject.name.ToLower().Replace(" ", "-");
                Button button = new Button() {
                    name = $"{elementName}-button",
                    text = lightGameObject.gameObject.name
                };
                button.AddToClassList(panelButtonClassName);

                if (lightGameObject.activeSelf) {
                    button.AddToClassList(selectedPanelButtonClassName);
                }

                button.RegisterCallback<ClickEvent>(OnLightButtonClicked);
                lightContainer.Add(button);
            }
        }

        private void OnLightButtonClicked(ClickEvent clickEvent) {
            var button = clickEvent.currentTarget as Button;
            lightingController.ToggleLight(button.text);
            button.ToggleInClassList(selectedPanelButtonClassName);
        }

        private void InitializeSkyboxButtons() {
            activeSkyboxButton = contentContainer.Q<Button>(defaultSkyboxButtonName);
            activeSkyboxButton.RegisterCallback<ClickEvent>(OnSkyboxButtonClicked);
            contentContainer.Q<Button>(darkSkyboxButtonName).RegisterCallback<ClickEvent>(OnSkyboxButtonClicked);
            contentContainer.Q<Button>(noSkyboxButtonName).RegisterCallback<ClickEvent>(OnSkyboxButtonClicked);
        }

        private void OnSkyboxButtonClicked(ClickEvent clickEvent) {
            var button = clickEvent.currentTarget as Button;
            if (button == activeSkyboxButton)
                return;
            
            activeSkyboxButton.RemoveFromClassList(selectedPanelButtonClassName);
            button.AddToClassList(selectedPanelButtonClassName);
            activeSkyboxButton = button;

            if (button.name == defaultSkyboxButtonName) {
                lightingController.SetDefaultSkybox();
            }
            else if(button.name == darkSkyboxButtonName) {
                lightingController.SetDarkSkybox();
            }
            else {
                lightingController.SetNoSkybox();    
            }
        }
    }
}