using System;
using System.Collections;
using System.Linq;
using MeshVisualizer.Controller;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class ModelAssetControllerTests {
    private GameObject gameObject { get; set; }
    private ModelAssetController testObject { get; set; }
    
    private const string modelAssetLabel = "Model";
    private const string materialAssetLabel = "Material";
    private const string textureAssetLabel = "Texture";

    [SetUp]
    public void Setup() {
        gameObject = new GameObject();
        testObject = gameObject.AddComponent<ModelAssetController>();
    }

    [TearDown]
    public void TearDown() {
        testObject = null;
        if (gameObject != null)
            Object.Destroy(gameObject);
    }
    
    [UnityTest]
    public IEnumerator SwitchModel_LoadsNewModel() {
        var lastModel = testObject.currentModel;
        
        var key = GetFirstAssetLocation(modelAssetLabel).PrimaryKey;
        testObject.SwitchModel(key);

        yield return WaitForCondition(() => (lastModel != testObject.currentModel), 1);
        
        Assert.AreNotEqual(lastModel, testObject.currentModel);
    }
    
    [UnityTest]
    public IEnumerator SwitchMaterial_LoadsNewMaterial() {
        var lastMaterial = testObject.currentMaterial;
        
        var key = GetFirstAssetLocation(materialAssetLabel).PrimaryKey;
        testObject.SwitchMaterial(key);
        
        yield return WaitForCondition(() => (lastMaterial != testObject.currentMaterial), 1);
        
        //make sure the current and previous materials are not the same
        Assert.AreNotEqual(lastMaterial, testObject.currentMaterial);
    }
    
    [UnityTest]
    public IEnumerator SwitchMaterial_SwitchesModelsMaterial() {
        yield return SwitchModel_LoadsNewModel();
        
        var meshRenderer = testObject.currentModel.GetComponent<MeshRenderer>();
        var lastModelMaterial = meshRenderer.material;
        
        var key = GetFirstAssetLocation(materialAssetLabel).PrimaryKey;
        testObject.SwitchMaterial(key);
        
        yield return WaitForCondition(() => (lastModelMaterial != meshRenderer.material), 1);
        
        //make sure the current and previous materials are not the same
        Assert.AreNotEqual(lastModelMaterial, meshRenderer.material);
    }
    
    [UnityTest]
    public IEnumerator SwitchTexture_LoadsNewTexture() {
        var lastTextureMaterial = testObject.currentTextureMaterial;
        
        var key = GetFirstAssetLocation(textureAssetLabel).PrimaryKey;
        testObject.SwitchTexture(key);
        
        yield return WaitForCondition(() => (lastTextureMaterial != testObject.currentTextureMaterial), 1);
        
        Assert.AreNotEqual(lastTextureMaterial, testObject.currentTextureMaterial);
    }
    
    [UnityTest]
    public IEnumerator SwitchTexture_SwitchesModelsTexture() {
        yield return SwitchModel_LoadsNewModel();
        yield return SwitchMaterial_LoadsNewMaterial();
        
        var key = GetFirstAssetLocation(textureAssetLabel).PrimaryKey;
        testObject.SwitchTexture(key);
        var lastTextureMaterial = testObject.currentTextureMaterial;
        
        yield return WaitForCondition(() => (lastTextureMaterial != testObject.currentTextureMaterial), 1);
        
        var meshRenderer = testObject.currentModel.GetComponent<MeshRenderer>();
        var modelMaterial = meshRenderer.material;
        
        string[] textureProperties = testObject.currentTextureMaterial.GetTexturePropertyNames();
        foreach (var texturePropertyName in textureProperties) {
            if (modelMaterial.HasTexture(texturePropertyName)) {
                Assert.AreEqual(modelMaterial.GetTexture(texturePropertyName), 
                    testObject.currentTextureMaterial.GetTexture(texturePropertyName));
            }
        }
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

    private IResourceLocation GetFirstAssetLocation(string assetLabel) {
        var handle = Addressables.LoadResourceLocationsAsync(assetLabel);
        handle.WaitForCompletion();
        var result = handle.Result.First();
        Addressables.Release(handle);
        return result;
    }
}
