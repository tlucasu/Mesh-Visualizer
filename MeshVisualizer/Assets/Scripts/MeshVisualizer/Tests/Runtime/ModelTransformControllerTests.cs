using MeshVisualizer.Controller;
using NUnit.Framework;
using UnityEngine;

public class ModelTransformControllerTests {

    private GameObject parent { get; set; }
    private ModelTransformController testObject { get; set; }

    private Vector3 worldPosition = Vector3.one;
    private Quaternion worldRotation = Quaternion.Euler(Vector3.up * 90);
    private Vector3 worldScale = Vector3.one;
    private ModelTransformController.MinMax constraint { get; } = new ModelTransformController.MinMax(-2, 2);

    [SetUp]
    public void Setup() {
        parent = new GameObject();
        
        GameObject child = new GameObject();
        child.transform.SetParent(parent.transform);

        parent.transform.position = worldPosition;
        parent.transform.rotation = worldRotation;
        parent.transform.localScale = worldScale;
        
        testObject = child.AddComponent<ModelTransformController>();
        
        //Initialize Constraints
        testObject.xPositionConstraint 
            = testObject.yPositionConstraint
                = testObject.zPositionConstraint = constraint;
    }

    [TearDown]
    public void TearDown() {
        testObject = null;
        if(parent != null)
            Object.DestroyImmediate(parent);
    }

    [TestCase(0,0,0)]
    [TestCase(0.2f,0.3f,0.4f)]
    [TestCase(-0.2f,0.3f,-0.4f)]
    [TestCase(-41,-10,-15.4f)]
    [TestCase(41,10,15.4f)]
    [TestCase(1,1,1)]
    [TestCase(1.1324f,-1.04f,1.30f)]
    public void SetLocalPositionWithConstraints(float x, float y, float z) {
        Vector3 targetPosition = new Vector3(x, y, z);
        
        testObject.SetLocalPositionWithConstraints(targetPosition);
        
        Vector3 expectedPosition = new Vector3() {
            x = Mathf.Clamp(targetPosition.x, constraint.min, constraint.max),
            y = Mathf.Clamp(targetPosition.y, constraint.min, constraint.max),
            z = Mathf.Clamp(targetPosition.z, constraint.min, constraint.max),
        };
        
        float mag = Vector3.SqrMagnitude(testObject.transform.localPosition - expectedPosition);
        Assert.That(Mathf.Approximately(mag, 0), $"Expected {targetPosition} got {testObject.transform.localPosition}");
    }
    
    [TestCase(0,0,0)]
    [TestCase(-90,90,-90)]
    [TestCase(90,180,-180)]
    public void SetLocalRotation(float x, float y, float z) {
        Vector3 targetRotation = new Vector3(x, y, z);
        
        testObject.SetLocalRotation(targetRotation);

        var expectedRotation = Quaternion.Euler(targetRotation);
        
        Assert.That(expectedRotation == testObject.transform.localRotation, $"Expected {expectedRotation} got {testObject.transform.localRotation}");
    }
    
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(2)]
    [TestCase(2.5f)]
    [TestCase(-1023.3425f)]
    public void SetLocalScale(float scale) {
        Vector3 targetScale = Vector3.one * scale;
        
        testObject.SetLocalScale(scale);
        
        float mag = Vector3.SqrMagnitude(testObject.transform.localScale - targetScale);
        Assert.That(Mathf.Approximately(mag, 0), $"Expected {targetScale} got {testObject.transform.localScale}");
    }
}
