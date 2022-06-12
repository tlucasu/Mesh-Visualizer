using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAssetController : MonoBehaviour {
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
        
    public void SwitchModel(string assetKey) {
        meshFilter.mesh = null;
    }

    public void SwitchTexture(string assetKey) {
        meshRenderer.material.mainTexture = null;
    }

    public void SwitchMaterial(string assetKey) {
        meshRenderer.material = null;
    }
}
