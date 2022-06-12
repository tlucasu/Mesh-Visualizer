using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MeshVisualizer {
    public class ModelAssetController : MonoBehaviour {
        private string currentModelAssetKey { get; set; }
        private string currentMaterialAssetKey { get; set; }
        private string currentTextureAssetKey { get; set; }
        
        private GameObject currentModel { get; set; }
        
        private MeshRenderer modelMeshRenderer { get; set; }
        private Texture currentTexture { get; set; }
        private Material currentMaterial { get; set; }


        public void SwitchModel(string assetKey) {
            if (currentModelAssetKey == assetKey)
                return;
            
            if (currentModel != null) {
                Addressables.ReleaseInstance(currentModel);
                modelMeshRenderer = null;
            }

            currentModelAssetKey = assetKey;
            Addressables.InstantiateAsync(assetKey, this.transform, false).Completed += OnModelLoadComplete;
        }

        private void OnModelLoadComplete(AsyncOperationHandle<GameObject> handle) {
            SetModel(handle.Result);
        }

        public void SwitchMaterial(string assetKey) {
            if (currentMaterialAssetKey == assetKey)
                return;
            
            if (currentMaterial != null) {
                Addressables.Release(currentMaterial);
            }

            currentMaterialAssetKey = assetKey;
            Addressables.LoadAssetAsync<Material>(assetKey).Completed += OnMaterialLoadComplete;
        }

        private void OnMaterialLoadComplete(AsyncOperationHandle<Material> handle) {
            SetMaterial(handle.Result);
        }

        public void SwitchTexture(string assetKey) {
            if (currentTextureAssetKey == assetKey)
                return;
            
            if (currentTexture != null) {
                Addressables.Release(currentTexture);
            }

            currentTextureAssetKey = assetKey;
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
