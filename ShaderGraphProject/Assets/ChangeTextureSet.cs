using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureSet : MonoBehaviour
{
    public Material NatureMaterial;
    public Texture NatureSprite;

    private void OnEnable()
    {
        NatureMaterial.SetTexture("_MainTex", NatureSprite);
    }
}
