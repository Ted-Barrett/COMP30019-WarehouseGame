using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingScript : MonoBehaviour
{
    [SerializeField]
    Material postProcessingMat;

    void Start() {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.DepthNormals;
    }

    // called after camera renders
    void OnRenderImage(RenderTexture src, RenderTexture dest) {

        // applies post processing effect to src and stores in dest
        Graphics.Blit(src, dest, postProcessingMat);
    }
}
