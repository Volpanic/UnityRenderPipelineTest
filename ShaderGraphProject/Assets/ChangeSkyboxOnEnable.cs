using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyboxOnEnable : MonoBehaviour
{
    public Material Skybox;

    private void OnEnable()
    {
        RenderSettings.skybox = Skybox;
    }
}
