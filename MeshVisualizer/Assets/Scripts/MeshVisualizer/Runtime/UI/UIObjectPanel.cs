using System.Collections.Generic;
using System.Linq;
using MeshVisualizer.Controller;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UIObjectPanel : UIPanel {
        private const string objectContainerName = "object-container";
        
        [SerializeField] 
        private AssetLabelReference assetLabel;

        [Header("Reference")] 
        [SerializeField] 
        private ModelAssetController modelAssetController;
        
        private VisualElement objectContainer { get; set; }
        private VisualElement selectedButton { get; set; }

        public bool initialized { get; private set; }
        
        protected void Start() {
            if (modelAssetController == null) {
                Debug.LogError($"Model Asset Controller in {this} must not be null. Cannot initialize panel");
                return;
            }

            if (assetLabel.labelString is not ("Model" or "Material" or "Texture")) {
                Debug.LogError($"Asset Label must be 'Model' or 'Material' or 'Texture' for {this}. Cannot initialize panel");
                return;
            }
            
            //using object container instead of content container for more flexibility if more
            //content is added to the object panel in the future
            objectContainer = contentContainer.Q<VisualElement>(objectContainerName);
            if (objectContainer == null) {
                Debug.LogError($"Unable to find '{objectContainerName}' in content container. Make sure the correct" +
                               $" VisualTreeAsset is assigned {this}.");
                return;
            }
            
            if (assetLabel.RuntimeKeyIsValid())
                Addressables.LoadResourceLocationsAsync(assetLabel.RuntimeKey).Completed += OnItemListComplete;
            else {
                Debug.LogError($"Asset Label for {this} is invalid");
            }
        }

        private void OnItemListComplete(AsyncOperationHandle<IList<IResourceLocation>> handle) {
            //initializes the object container
            objectContainer.Clear(); 
            
            if (handle.Result.Count == 0) {
                Debug.LogWarning($"No objects were found with the label '{assetLabel.RuntimeKey}'");
                return;
            }

            List<string> keys = handle.Result
                                      .Select(x => x.PrimaryKey)
                                      .ToHashSet()  //strips out any duplicate keys
                                      .ToList();
           
            //Sets the keys to be in alphabetical order
            keys.Sort();

            string currentlySelectedAsset = GetCurrentAssetKey(assetLabel.labelString);
            
            //Populate content container
            foreach(var key in keys){
                Button itemButton = new Button() {
                    name = $"{key}-button",
                    text = key
                };
                itemButton.AddToClassList(panelButtonClassName);

                if (key == currentlySelectedAsset) 
                    SelectButton(itemButton);

                itemButton.RegisterCallback<ClickEvent>(ItemClicked);

                contentContainer.Add(itemButton);
            }
            
            Addressables.Release(handle);
            initialized = true;
        }

        private void ItemClicked(ClickEvent clickEvent) {
            Button itemButton = clickEvent.currentTarget as Button;
            SelectButton(itemButton);
            SwitchAsset(itemButton.text);
        }

        private void SelectButton(VisualElement itemElement) {
            if(selectedButton != null)
                selectedButton.RemoveFromClassList(selectedPanelButtonClassName);
            
            itemElement.AddToClassList(selectedPanelButtonClassName);
            selectedButton = itemElement;
        }
        

        /// <summary>
        /// Given an Addressable Asset Label, returns the current asset key used for that asset
        /// </summary>
        public string GetCurrentAssetKey(string assetLabelString) {
            if (assetLabelString == "Model")
                return modelAssetController.modelAssetKey;
            if (assetLabelString == "Material")
                return modelAssetController.materialAssetKey;
            if (assetLabelString == "Texture")
                return modelAssetController.textureAssetKey;
            
            Debug.LogError($"Cannot find asset key for '{assetLabelString}' asset label");
            return null;
        }

        private void SwitchAsset(string assetKey) {
            string assetLabelString = assetLabel.labelString;
            if (assetLabelString == "Model")
                modelAssetController.SwitchModel(assetKey);
            else if (assetLabelString == "Material")
                modelAssetController.SwitchMaterial(assetKey);
            else if (assetLabelString == "Texture")
                modelAssetController.SwitchTexture(assetKey);
        }
    }
}