using System;
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
            //waiting for the panel to initialize
            yield return WaitForCondition(() => modelPanel.initialized, 1);
            //wait for asset controller to finish loading starting asset
            yield return WaitForCondition(() => assetController.currentModel != null, 1);
            
            GameObject previousModel = assetController.currentModel;
            Button objectItemButton = modelPanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return WaitForCondition(() => assetController.currentModel != previousModel, 1);
            
            Assert.AreNotEqual(assetController.currentModel, previousModel);
        }
        
        [UnityTest]
        public IEnumerator OnClick_MaterialPanelItem_ChangesMaterialOnModel() {
            //waiting for the panel to initialize
            yield return WaitForCondition(() => materialPanel.initialized, 1);
            //wait for asset controller to finish loading starting asset
            yield return WaitForCondition(() => assetController.currentMaterial != null, 1);
            
            Material previousMaterial = assetController.currentMaterial;
            Button objectItemButton = materialPanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return WaitForCondition(() => assetController.currentMaterial != previousMaterial, 1);
            
            Assert.AreNotEqual(assetController.currentMaterial, previousMaterial);
        }
        
        [UnityTest]
        public IEnumerator OnClick_TexturePanelItem_ChangesTextureOnModel() {
            //waiting for the panel to initialize
            yield return WaitForCondition(() => texturePanel.initialized, 1);
            //wait for asset controller to finish loading starting asset
            yield return WaitForCondition(() => assetController.currentTextureMaterial != null, 1);
            
            Material previousTextureMaterial = assetController.currentTextureMaterial;
            Button objectItemButton = texturePanel.contentContainer.Q<Button>(className:objectItemClass);
            
            SendClickEvent(objectItemButton);
            
            yield return WaitForCondition(() => assetController.currentTextureMaterial != previousTextureMaterial, 1);
            
            Assert.AreNotEqual(assetController.currentTextureMaterial, previousTextureMaterial);
        }
        
        private IEnumerator WaitForCondition(Func<bool> condition, float timeout) {
            var startTime = Time.timeSinceLevelLoad;
            while (!condition.Invoke() 
                   && startTime + timeout > Time.timeSinceLevelLoad) {
                yield return null;
            }

            if (startTime + timeout < Time.timeSinceLevelLoad) {
                throw new TimeoutException();
            }
        }
    }
}