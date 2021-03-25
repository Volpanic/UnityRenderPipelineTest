using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcesser : MonoBehaviour
{
    public Material PostProcessMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source,destination,PostProcessMaterial);
    }
}
