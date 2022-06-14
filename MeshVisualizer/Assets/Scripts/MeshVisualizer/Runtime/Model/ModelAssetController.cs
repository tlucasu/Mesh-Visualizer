using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MeshVisualizer {
    public class ModelAssetController : MonoBehaviour {

        [SerializeField]
        [AssetReferenceUILabelRestriction("Model")]
        private AssetReferenceGameObject startingModel;
        
        [SerializeField]
        [AssetReferenceUILabelRestriction("Material")]
        private AssetReference startingMaterial;
        
        [SerializeField]
        [AssetReferenceUILabelRestriction("Texture")]
        private AssetReference startingTexture;
        
        private string currentModelAssetKey { get; set; }
        public GameObject currentModel { get; private set; }
        private MeshRenderer modelMeshRenderer { get; set; }
        
        
        private string currentMaterialAssetKey { get; set; }
        public Material currentMaterial { get;  private set; }
        
        
        private string currentTextureAssetKey { get; set; }
        public Material currentTextureMaterial { get; private set; }

        private bool destroyed { get; set; } = false;

        private void Awake() {
            if (startingModel != null && startingModel.RuntimeKeyIsValid()) {
                Addressables.LoadResourceLocationsAsync(startingModel).Completed += handle => {
                    IResourceLocation resourceLocation = handle.Result.First();
                    SwitchModel(resourceLocation.PrimaryKey);
                    Addressables.Release(handle);
                };
            }

            if (startingMaterial != null && startingMaterial.RuntimeKeyIsValid()) {
                Addressables.LoadResourceLocationsAsync(startingMaterial).Completed += handle => {
                    IResourceLocation resourceLocation = handle.Result.First();
                    SwitchMaterial(resourceLocation.PrimaryKey);
                    Addressables.Release(handle);
                };
            }

            if (startingTexture != null && startingTexture.RuntimeKeyIsValid()) {
                Addressables.LoadResourceLocationsAsync(startingTexture).Completed += handle => {
                    IResourceLocation resourceLocation = handle.Result.First();
                    SwitchTexture(resourceLocation.PrimaryKey);
                    Addressables.Release(handle);
                };
            }
        }

        private void OnDestroy() {
            destroyed = true;
        }


        public void SwitchModel(string assetKey) {
            if (currentModelAssetKey == assetKey)
                return;
            
            currentModelAssetKey = assetKey;
            Addressables.InstantiateAsync(assetKey).Completed += handle => {
                if (destroyed) {
                    Addressables.Release(handle);
                    return;
                }

                GameObject instance = handle.Result;
                instance.transform.SetParent(this.transform, false);
                SetModel(instance);
            };
        }

        public void SwitchMaterial(string assetKey) {
            if (currentMaterialAssetKey == assetKey)
                return;

            currentMaterialAssetKey = assetKey;
            Addressables.LoadAssetAsync<Material>(assetKey).Completed += handle => {
                if (destroyed) {
                    Addressables.Release(handle);
                    return;
                }
                
                Material instance = Instantiate(handle.Result);
                SetMaterial(instance);
                Addressables.ReleaseInstance(handle);
            };
        }

        public void SwitchTexture(string assetKey) {
            if (currentTextureAssetKey == assetKey)
                return;

            currentTextureAssetKey = assetKey;
            Addressables.LoadAssetAsync<Material>(assetKey).Completed += handle => {
                if (destroyed) {
                    Addressables.Release(handle);
                    return;
                }
                
                SetTexture(handle.Result);
            };
        }
        
        private void SetModel(GameObject model) {
            if (currentModel != null) {
                Addressables.ReleaseInstance(currentModel);
                modelMeshRenderer = null;
            }
            
            currentModel = model;

            if (model == null)
                return;

            modelMeshRenderer = model.GetComponent<MeshRenderer>();
            SetMaterial(currentMaterial);
        }

        private void SetMaterial(Material material) {
            if (material != currentMaterial && currentMaterial != null) {
                DestroyImmediate(currentMaterial);
            }

            currentMaterial = material;
            
            if (modelMeshRenderer == null)
                return;

            modelMeshRenderer.material = material;
            
            SetTexture(currentTextureMaterial);
        }

        private void SetTexture(Material textureMaterial) {
            if (textureMaterial != currentTextureMaterial && currentTextureMaterial != null) {
                Addressables.Release(currentTextureMaterial);
            }
            
            ClearTextures();

            currentTextureMaterial = textureMaterial;

            if (currentMaterial == null || textureMaterial == null)
                return;

            string[] textureProperties = textureMaterial.GetTexturePropertyNames();
            foreach (var textureProperty in textureProperties) {
                if (currentMaterial.HasTexture(textureProperty)) {
                    Texture texture = textureMaterial.GetTexture(textureProperty);
                    currentMaterial.SetTexture(textureProperty, texture);
                }
            }
        }

        private void ClearTextures() {
            if (currentMaterial == null)
                return;
            
            string[] textureProperties = currentMaterial.GetTexturePropertyNames();
            foreach (var textureProperty in textureProperties) {
                currentMaterial.SetTexture(textureProperty, null);
            }
        }
    }
}
