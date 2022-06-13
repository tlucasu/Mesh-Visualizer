using NUnit.Framework;
using UnityEngine;

public class ModelTransformControllerTests {

    private GameObject testObject { get; set; }
    private ModelTransformController controller { get; set; }

    [SetUp]
    public void Setup() {
        testObject = new GameObject();
        controller = testObject.AddComponent<ModelTransformController>();
    }

    [TearDown]
    public void TearDown() {
        controller = null;
        if(testObject != null)
            Object.DestroyImmediate(testObject);
    }

    [TestCase(0,0,0)]
    [TestCase(-1,0,0)]
    [TestCase(-1,-10,-15.4f)]
    [TestCase(1,1,1)]
    [TestCase(1.1324f,-1.04f,1.30f)]
    public void SetPosition(float x, float y, float z) {
        Vector3 targetPosition = new Vector3(x, y, z);
        controller.SetRotation(targetPosition);
        
        float mag = Vector3.SqrMagnitude(testObject.transform.localRotation.eulerAngles - targetPosition);
        Assert.That(Mathf.Approximately(mag, 0), $"Expected {targetPosition} got {testObject.transform.localPosition}");
    }
    
    [TestCase(0,0,0)]
    [TestCase(-90,90,-90)]
    [TestCase(90,180,-180)]
    public void SetRotation(float x, float y, float z) {
        Vector3 targetRotation = new Vector3(x, y, z);
        controller.SetRotation(targetRotation);
        
        float mag = Vector3.SqrMagnitude(testObject.transform.localRotation.eulerAngles - targetRotation);
        Assert.That(Mathf.Approximately(mag, 0), $"Expected {targetRotation} got {testObject.transform.localRotation.eulerAngles}");
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(2)]
    [TestCase(2.5f)]
    [TestCase(-1023.3425f)]
    public void SetScale(float scale) {
        Vector3 targetScale = Vector3.one * scale;
        controller.SetScale(scale);
        float mag = Vector3.SqrMagnitude(testObject.transform.localScale - targetScale);
        Assert.That(Mathf.Approximately(mag, 0), $"Expected {targetScale} got {testObject.transform.localScale}");
    }
}
