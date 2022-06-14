using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UIObjectPanel : UIPanel {
        private const string objectContainerName = "object-container";
        private const string objectItemClass = "object-item";
        private const string selectedObjectItemClass = "selected-object-item";
        
        [SerializeField] 
        private AssetLabelReference assetLabel;

        [Header("Events")]
        [Tooltip("When an asset element is clicked, trigger event")]
        public UnityEvent<string> onAssetClick;
        
        private VisualElement objectContainer { get; set; }
        private VisualElement lastSelectedItem { get; set; }
        
        protected void Start() {
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
            
            //Populate content container
            foreach(var key in keys){
                Button itemButton = new Button() {
                    name = $"{key}-button",
                    text = key
                };
                itemButton.AddToClassList(objectItemClass);

                itemButton.clicked += () => {
                    SelectItem(itemButton);
                    onAssetClick?.Invoke(key);
                };

                contentContainer.Add(itemButton);
            }
            
            Addressables.Release(handle);
        }

        private void SelectItem(VisualElement itemElement) {
            if(lastSelectedItem != null)
                lastSelectedItem.RemoveFromClassList(selectedObjectItemClass);
            
            itemElement.AddToClassList(selectedObjectItemClass);
            lastSelectedItem = itemElement;
        }
    }
}