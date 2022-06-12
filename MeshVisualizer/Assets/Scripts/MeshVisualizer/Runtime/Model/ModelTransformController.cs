using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTransformController : MonoBehaviour
{
    public void SetHorizontalPosition(float x) {
        //Not caching transform as its no longer need for performance gains
        var positions = transform.localPosition;
        positions.x = x;
        transform.localPosition = positions;
    }
    public void SetVerticalPosition(float y) {
        //Not caching transform as its no longer need for performance gains
        var position = transform.localPosition;
        position.y = y;
        transform.localPosition = position;
    }
    
    public void SetScale(float scale) {
        transform.localScale = Vector3.one * scale;
    }
}
