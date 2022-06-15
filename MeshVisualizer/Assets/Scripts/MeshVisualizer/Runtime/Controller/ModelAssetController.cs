using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MeshVisualizer.Controller {
    /// <summary>
    /// Behaviour attached to the model base that handles switching of the 3D model, material and texture.
    /// Textures are loaded via materials then copied from one material to the other using the materials' texture property names.
    /// </summary>
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
        
        internal string modelAssetKey { get; private set; }
        /// <summary>
        /// Reference to the current model loaded that is loaded
        /// </summary>
        public GameObject currentModel { get; private set; }
        private MeshRenderer modelMeshRenderer { get; set; }
        
        
        internal string materialAssetKey { get; private set; }
        /// <summary>
        /// Reference to an instance of the current material that is loaded
        /// </summary>
        public Material currentMaterial { get;  private set; }
        
        
        internal string textureAssetKey { get; private set; }
        /// <summary>
        /// Reference to the current 'texture' material that is loaded
        /// </summary>
        public Material currentTextureMaterial { get; private set; }
        
        /// <summary>
        /// Reference to the current main texture that used on the model
        /// </summary>
        public Texture currentTexture { get; private set; }

        private bool destroyed { get; set; } = false;

        private void Awake() {
            //Loading in the same manner that UIObjectPanel sends keys to be consistent
            if (startingModel != null && startingModel.RuntimeKeyIsValid()) {
                //Load starting model
                Addressables.LoadResourceLocationsAsync(startingModel).Completed += handle => {
                    IResourceLocation resourceLocation = handle.Result.First();
                    SwitchModel(resourceLocation.PrimaryKey);
                    Addressables.Release(handle);
                };
            }

            if (startingMaterial != null && startingMaterial.RuntimeKeyIsValid()) {
                //Load starting material
                Addressables.LoadResourceLocationsAsync(startingMaterial).Completed += handle => {
                    IResourceLocation resourceLocation = handle.Result.First();
                    SwitchMaterial(resourceLocation.PrimaryKey);
                    Addressables.Release(handle);
                };
            }

            if (startingTexture != null && startingTexture.RuntimeKeyIsValid()) {
                //Load starting texture
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

        /// <summary>
        /// Switches the model to an instance of the model loaded with the addressable address key
        /// </summary>
        public void SwitchModel(string addressableAddressKey) {
            if (modelAssetKey == addressableAddressKey)
                return;
            
            modelAssetKey = addressableAddressKey;
            Addressables.InstantiateAsync(addressableAddressKey).Completed += handle => {
                if (destroyed || modelAssetKey != addressableAddressKey) {
                    Addressables.Release(handle);
                    return;
                }

                GameObject instance = handle.Result;
                instance.transform.SetParent(this.transform, false);
                instance.layer = LayerMask.NameToLayer("Model"); //set the objects layer to model to avoid being in the reflection probe
                SetModel(instance);
            };
        }

        /// <summary>
        /// Switches the model's material to an instance of the material loaded with the addressable address key 
        /// </summary>
        public void SwitchMaterial(string addressableAddressKey) {
            if (materialAssetKey == addressableAddressKey)
                return;

            materialAssetKey = addressableAddressKey;
            Addressables.LoadAssetAsync<Material>(addressableAddressKey).Completed += handle => {
                if (destroyed || materialAssetKey != addressableAddressKey) {
                    Addressables.Release(handle);
                    return;
                }
                
                Material instance = Instantiate(handle.Result);
                SetModelMaterial(instance);
                Addressables.ReleaseInstance(handle);
            };
        }

        /// <summary>
        /// Switches model's texture to the texture loaded with the addressable address key
        /// </summary>
        public void SwitchTexture(string addressableAddressKey) {
            if (textureAssetKey == addressableAddressKey)
                return;

            textureAssetKey = addressableAddressKey;
            Addressables.LoadAssetAsync<Object>(addressableAddressKey).Completed += handle => {
                if (destroyed || textureAssetKey != addressableAddressKey) {
                    Addressables.Release(handle);
                    return;
                }
                
                if(handle.Result is Material material)
                    SetModelTexture(material);
                else if(handle.Result is Texture texture)
                    SetModelTexture(texture);
            };
        }
        
        /// <summary>
        /// Sets the model
        /// </summary>
        private void SetModel(GameObject model) {
            if (currentModel != null) {
                Addressables.ReleaseInstance(currentModel);
                modelMeshRenderer = null;
            }
            
            currentModel = model;

            if (model == null)
                return;

            modelMeshRenderer = model.GetComponent<MeshRenderer>();
            SetModelMaterial(currentMaterial);
        }
        
        /// <summary>
        /// Sets the material of the model
        /// </summary>
        private void SetModelMaterial(Material material) {
            if (material != currentMaterial && currentMaterial != null) {
                DestroyImmediate(currentMaterial);
            }

            currentMaterial = material;
            
            if (modelMeshRenderer == null)
                return;

            modelMeshRenderer.material = material;
            
            if(currentTextureMaterial != null)
                SetModelTexture(currentTextureMaterial);
            else
                SetModelTexture(currentTexture);
        }

        /// <summary>
        /// Sets the texture on the model's material by copying the textures from the source material
        /// </summary>
        private void SetModelTexture(Material source) {
            if (currentTexture != null) {
                Addressables.Release(currentTexture);
                currentTexture = null;
            }
            
            if (source != currentTextureMaterial && currentTextureMaterial != null) {
                Addressables.Release(currentTextureMaterial);
            }
            
            ClearTextures(currentMaterial);
            
            currentTextureMaterial = source;
            
            CopyTextures(source, currentMaterial);
        }
        
        /// <summary>
        /// Sets the texture on the model's material by copying the textures from the source texture
        /// </summary>
        private void SetModelTexture(Texture texture) {
            if (currentTextureMaterial != null) {
                Addressables.Release(currentTextureMaterial);
                currentTextureMaterial = null;
            }

            if (currentTexture != null) {
                Addressables.Release(currentTexture);
            }
            
            ClearTextures(currentMaterial);
            
            currentTexture = texture;

            if(currentMaterial != null)
                currentMaterial.mainTexture = texture;
        }

        /// <summary>
        /// Copies all textures from the source material to the destination material, provided they have the same property name.
        /// </summary>
        private void CopyTextures(Material source, Material destination) {
            if (source == null || destination == null)
                return;
            
            string[] propertyNames = source.GetTexturePropertyNames();
            foreach (var propertyName in propertyNames) {
                if (destination.HasTexture(propertyName)) {
                    Texture texture = source.GetTexture(propertyName);
                    destination.SetTexture(propertyName, texture);
                }
            }
        }

        /// <summary>
        /// Sets all textures on the material to null
        /// </summary>
        private void ClearTextures(Material material) {
            if (material == null)
                return;
            
            string[] textureProperties = material.GetTexturePropertyNames();
            foreach (var textureProperty in textureProperties) {
                material.SetTexture(textureProperty, null);
            }
        }
    }
}
