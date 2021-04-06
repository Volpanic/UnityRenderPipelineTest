using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fades in at creation, meant to 
// run at start of the scene for smooth transition

public class FadeIn : MonoBehaviour
{
    //Fade info
    public Image FadeInImage;
    private Color fadeOutColor = new Color(0, 0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        FadeInImage.color = fadeOutColor;
    }

    // Update is called once per frame
    void Update()
    {
        fadeOutColor.a = Mathf.MoveTowards(fadeOutColor.a, 0, Time.deltaTime);
        FadeInImage.color = fadeOutColor;
    }
}
