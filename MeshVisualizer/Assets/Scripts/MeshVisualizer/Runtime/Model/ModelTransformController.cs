using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTransformController : MonoBehaviour
{
    public void SetPosition(Vector3 newPosition) {
        //Local position is used to allow for the model anchor to be nested in another
        //game object and be localed else where in the world 
        transform.localPosition = newPosition;
    }
    
    public void SetRotation(Vector3 newRotation) {
        transform.localRotation = Quaternion.Euler(newRotation);
    }
    
    public void SetScale(float scale) {
        transform.localScale = Vector3.one * scale;
    }
}
