using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MeshVisualizer {
    public class ModelAssetController : MonoBehaviour {
        [SerializeField, AssetReferenceUILabelRestriction("Model")]
        private AssetReferenceGameObject defaultModel;

        [SerializeField, AssetReferenceUILabelRestriction("Texture")]
        private AssetReferenceTexture defaultTexture;

        [SerializeField, AssetReferenceUILabelRestriction("Material")]
        private AssetReferenceT<Material> defaultMaterial;

        private GameObject currentModel { get; set; }
        private MeshRenderer modelMeshRenderer { get; set; }
        private Texture currentTexture { get; set; }
        private Material currentMaterial { get; set; }

        private void Awake() {
            if (defaultModel.RuntimeKeyIsValid())
                defaultModel.InstantiateAsync(this.transform, false).Completed += OnModelLoadComplete;

            if (defaultMaterial.RuntimeKeyIsValid())
                defaultMaterial.LoadAssetAsync().Completed += OnMaterialLoadComplete;

            if (defaultTexture.RuntimeKeyIsValid())
                defaultTexture.LoadAssetAsync().Completed += OnTextureLoadComplete;
        }

        public void SwitchModel(string assetKey) {
            if (currentModel != null) {
                Addressables.ReleaseInstance(currentModel);
                modelMeshRenderer = null;
            }

            Addressables.InstantiateAsync(assetKey, this.transform, false).Completed += OnModelLoadComplete;
        }

        private void OnModelLoadComplete(AsyncOperationHandle<GameObject> handle) {
            SetModel(handle.Result);
        }

        public void SwitchMaterial(string assetKey) {
            if (currentMaterial != null) {
                Addressables.Release(currentMaterial);
            }

            Addressables.LoadAssetAsync<Material>(assetKey).Completed += OnMaterialLoadComplete;
        }

        private void OnMaterialLoadComplete(AsyncOperationHandle<Material> handle) {
            SetMaterial(handle.Result);
        }

        public void SwitchTexture(string assetKey) {
            if (currentTexture != null) {
                Addressables.Release(currentTexture);
            }

            Addressables.LoadAssetAsync<Texture>(assetKey).Completed += OnTextureLoadComplete;
        }

        private void OnTextureLoadComplete(AsyncOperationHandle<Texture> handle) {
            SetTexture(handle.Result);
        }

        private void SetModel(GameObject model) {
            currentModel = model;

            if (model == null)
                return;

            modelMeshRenderer = model.GetComponent<MeshRenderer>();
            SetMaterial(currentMaterial);
        }

        private void SetMaterial(Material material) {
            currentMaterial = material;

            if (modelMeshRenderer == null)
                return;

            modelMeshRenderer.material = material;
            SetTexture(currentTexture);
        }

        private void SetTexture(Texture texture) {
            currentTexture = texture;
            if (modelMeshRenderer == null)
                return;

            modelMeshRenderer.material.mainTexture = texture;
        }
    }
}
