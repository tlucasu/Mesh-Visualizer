using System.Collections;
using System.Linq;
using MeshVisualizer;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.TestTools;

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
    public IEnumerator SwitchModel() {
        var currentModel = testObject.currentModel;
        
        var key = GetFirstAssetLocation(modelAssetLabel).PrimaryKey;
        testObject.SwitchModel(key);

        yield return new WaitForSecondsRealtime(1);
        
        Assert.AreNotEqual(currentModel, testObject.currentModel);
    }
    
    [UnityTest]
    public IEnumerator SwitchMaterial() {
        var currentMaterial = testObject.currentMaterial;
        
        var key = GetFirstAssetLocation(materialAssetLabel).PrimaryKey;
        testObject.SwitchMaterial(key);
        
        yield return new WaitForSecondsRealtime(1);
        
        Assert.AreNotEqual(currentMaterial, testObject.currentMaterial);
    }
    
    [UnityTest]
    public IEnumerator SwitchTexture() {
        var currentMaterial = testObject.currentTextureMaterial;
        
        var key = GetFirstAssetLocation(textureAssetLabel).PrimaryKey;
        testObject.SwitchTexture(key);
        
        yield return new WaitForSecondsRealtime(1);
        
        Assert.AreNotEqual(currentMaterial, testObject.currentTextureMaterial);
    }

    private IResourceLocation GetFirstAssetLocation(string assetLabel) {
        var handle = Addressables.LoadResourceLocationsAsync(assetLabel);
        handle.WaitForCompletion();
        var result = handle.Result.First();
        Addressables.Release(handle);
        return result;
    }
}
