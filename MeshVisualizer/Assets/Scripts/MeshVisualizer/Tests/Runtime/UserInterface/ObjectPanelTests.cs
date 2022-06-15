using System;
using System.Collections;
using MeshVisualizer.Controller;
using MeshVisualizer.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Scene.Main.UserInterface {
    public class ObjectPanelTests : UserInterfaceTestBase {
        private const string buttonClassName = "panel-button";
        private const string selectedButtonClassName = "selected-panel-button";

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
            yield return WaitForCondition(() => modelPanel.initialized, 
                1, "Model Panel never initialized");
            //wait for asset controller to finish loading starting asset
            yield return WaitForCondition(() => assetController.currentModel != null,
                1, "Initial model never loaded");

            GameObject previousModel = assetController.currentModel;
            
            //Find first button that isn't 'selected'
            Button objectItemButton = modelPanel.contentContainer
                                                .Query<Button>(className: buttonClassName)
                                                .ToList()
                                                .Find(x=> !x.ClassListContains(selectedButtonClassName));

            SendClickEvent(objectItemButton);
            
            yield return WaitForCondition(() => assetController.currentModel != previousModel, 
                1, "Model never changed");
            
            Assert.AreNotEqual(assetController.currentModel, previousModel);
        }
        
        [UnityTest]
        public IEnumerator OnClick_MaterialPanelItem_ChangesMaterialOnModel() {
            //waiting for the panel to initialize
            yield return WaitForCondition(() => materialPanel.initialized, 1);
            //wait for asset controller to finish loading starting asset
            yield return WaitForCondition(() => assetController.currentMaterial != null, 1);
            
            Material previousMaterial = assetController.currentMaterial;
            
            //Find first button that isn't 'selected'
            Button objectItemButton = materialPanel.contentContainer
                                                   .Query<Button>(className: buttonClassName)
                                                   .ToList()
                                                   .Find(x=> !x.ClassListContains(selectedButtonClassName));
            
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
            
            //Find first button that isn't 'selected'
            Button objectItemButton = texturePanel.contentContainer
                                                   .Query<Button>(className: buttonClassName)
                                                   .ToList()
                                                   .Find(x=> !x.ClassListContains(selectedButtonClassName));
            
            SendClickEvent(objectItemButton);
            
            yield return WaitForCondition(() => assetController.currentTextureMaterial != previousTextureMaterial, 1);
            
            Assert.AreNotEqual(assetController.currentTextureMaterial, previousTextureMaterial);
        }
        
        private IEnumerator WaitForCondition(Func<bool> condition, float timeout, string exceptionMessage = null) {
            var startTime = Time.timeSinceLevelLoad;
            while (!condition.Invoke() 
                   && startTime + timeout > Time.timeSinceLevelLoad) {
                yield return null;
            }

            if (startTime + timeout < Time.timeSinceLevelLoad) {
                throw new TimeoutException(exceptionMessage);
            }
        }
    }
}