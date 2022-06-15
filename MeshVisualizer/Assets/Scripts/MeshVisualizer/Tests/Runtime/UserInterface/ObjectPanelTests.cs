using System.Collections;
using MeshVisualizer;
using MeshVisualizer.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Scene.Main.UserInterface {
    public class ObjectPanelTests : UserInterfaceTestBase {
        private const string objectItemClass = "object-item";

        private UIObjectPanel modelPanel { get; set; }
        private UIObjectPanel materialPanel { get; set; }
        private UIObjectPanel texturePanel { get; set; }
        
        private ModelAssetController assetController { get; set; }
        
        protected override IEnumerator Setup() {
            yield return base.Setup();
            
            var modePanelGameObject = GameObject.Find("Model Panel");
            modelPanel = modePanelGameObject.GetComponent<UIObjectPanel>();
            
            var materialPanelGameObject = GameObject.Find("Material Panel");
            materialPanel = materialPanelGameObject.GetComponent<UIObjectPanel>();
            
            var texturePanelGameObject = GameObject.Find("Texture Panel");
            texturePanel = texturePanelGameObject.GetComponent<UIObjectPanel>();
            
            var modelGameObject = GameObject.Find("Model Anchor");
            assetController = modelGameObject.GetComponent<ModelAssetController>();
        }
        
        [UnityTest]
        public IEnumerator OnClick_ModelPanelItem_ChangesModel() {
            while (!modelPanel.initialized)
                yield return null;

            while (assetController.currentModel == null)
                yield return null;
            
            GameObject previousModel = assetController.currentModel;
            Button objectItemButton = modelPanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return new WaitForSecondsRealtime(1);
            
            Assert.AreNotEqual(assetController.currentModel, previousModel);
        }
        
        [UnityTest]
        public IEnumerator OnClick_MaterialPanelItem_ChangesMaterialOnModel() {
            while (!materialPanel.initialized)
                yield return null;

            while (assetController.currentMaterial == null)
                yield return null;
            
            Material previousMaterial = assetController.currentMaterial;
            Button objectItemButton = materialPanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return new WaitForSecondsRealtime(1);
            
            Assert.AreNotEqual(assetController.currentMaterial, previousMaterial);
        }
        
        [UnityTest]
        public IEnumerator OnClick_TexturePanelItem_ChangesTextureOnModel() {
            while (!texturePanel.initialized)
                yield return null;

            while (assetController.currentTextureMaterial == null)
                yield return null;
            
            Material previousTextureMaterial = assetController.currentTextureMaterial;
            Button objectItemButton = texturePanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return new WaitForSecondsRealtime(1);
            
            Assert.AreNotEqual(assetController.currentTextureMaterial, previousTextureMaterial);
        }
    }
}