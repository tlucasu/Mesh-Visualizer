using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LightingController : MonoBehaviour {
    public ReflectionProbe reflectionProbe;
    public Transform additionalLightContainer;
    
    [Header("Default Skybox")]
    public Material defaultSkybox;
    [ColorUsage(false, true)][InspectorName("Ambient Light")]
    public Color defaultSkyboxAmbientLight = Color.gray;
    [InspectorName("Directional Light")]
    public Light defaultDirectionalLight;
    
    [Header("Dark Skybox")][InspectorName("Skybox")]
    public Material darkSkybox;
    [ColorUsage(false, true)][InspectorName("Ambient Light")]
    public Color darkSkyboxAmbientLight = Color.gray;
    [InspectorName("Directional Light")]
    public Light darkDirectionalLight;
    
    [Header("No Skybox")]
    [InspectorName("Ambient Light")][ColorUsage(false, true)]
    public Color noSkyboxAmbientLight = Color.gray;

    public List<GameObject> additionalLights { get; private set; } = new();
    
    private void Awake() {
        SetDefaultSkybox();
        
        if (additionalLightContainer != null) {
            int lightCount = additionalLightContainer.childCount;
            for (int i = 0; i < lightCount; i++) {
                additionalLights.Add(additionalLightContainer.GetChild(i).gameObject);
            }
        }
    }

    [ContextMenu("Set Default Skybox")]
    public void SetDefaultSkybox() {
        RenderSettings.skybox = defaultSkybox;
        RenderSettings.ambientLight = defaultSkyboxAmbientLight;
        defaultDirectionalLight.gameObject.SetActive(true);
        darkDirectionalLight.gameObject.SetActive(false);
        if(reflectionProbe != null)
            reflectionProbe.RenderProbe();
        //Update the reflection probe
    }

    [ContextMenu("Set Dark Skybox")]
    public void SetDarkSkybox() {
        RenderSettings.skybox = darkSkybox;
        RenderSettings.ambientLight = darkSkyboxAmbientLight;
        defaultDirectionalLight.gameObject.SetActive(false);
        darkDirectionalLight.gameObject.SetActive(true);
        if(reflectionProbe != null)
            reflectionProbe.RenderProbe();
    }

    [ContextMenu("Set No Skybox")]
    public void SetNoSkybox() {
        RenderSettings.skybox = null;
        RenderSettings.ambientLight = noSkyboxAmbientLight;
        defaultDirectionalLight.gameObject.SetActive(false);
        darkDirectionalLight.gameObject.SetActive(false);
        if(reflectionProbe != null)
            reflectionProbe.RenderProbe();
    }

    public void ToggleLight(string lightGameObjectName) {
        var lightGameObject = additionalLights.Find(x => x.name == lightGameObjectName);
        lightGameObject.SetActive(!lightGameObject.activeSelf);
    }
}
