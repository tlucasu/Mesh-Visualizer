using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

namespace MeshVisualizer.UI {
    public class UIObjectPanel : UIPanel {
        [SerializeField] private AssetLabelReference assetLabel;

        [Tooltip("When an asset is clicked, call event")]
        public UnityEvent<string> onAssetClick;

        protected void Start() {
            if (assetLabel.RuntimeKeyIsValid())
                Addressables.LoadResourceLocationsAsync(assetLabel.RuntimeKey).Completed += OnItemListComplete;
            else {
                Debug.LogError($"Asset Label for {this} is invalid");
            }
        }

        private void OnItemListComplete(AsyncOperationHandle<IList<IResourceLocation>> obj) {
            if (obj.Result.Count == 0) {
                Debug.LogWarning($"No objects were found with the label '{assetLabel.RuntimeKey}'");
                return;
            }

            //Populate content container
            foreach (var result in obj.Result) {
                string key = result.PrimaryKey;

                Button itemButton = new Button() {
                    name = $"{key.ToLower()}-button",
                    text = key
                };

                itemButton.clicked += () => { onAssetClick.Invoke(key); };

                contentContainer.Add(itemButton);
            }

            // contentContainer.MarkDirtyRepaint();
        }
    }
}