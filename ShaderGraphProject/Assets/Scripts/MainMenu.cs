using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Controls main menu functions
// and fading out to game scene.

public class MainMenu : MonoBehaviour
{
    //Fade Info
    public Image FadeOutImage;
    private bool fadingOut;
    private Color fadeOutColor = new Color(0,0,0,0);

    //Lerps the fade out images alpha, if needs to fade out
    void Update()
    {
        if(fadingOut)
        {
            fadeOutColor.a = Mathf.MoveTowards(fadeOutColor.a,1,Time.deltaTime);
            FadeOutImage.color = fadeOutColor;

            // Change scene if fadeout done
            if(fadeOutColor.a == 1)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

    //Triggered by button on click event, starts fadeout.
    public void PlayGame()
    {
        fadingOut = true;
    }

    //Triggered by button on click event, quits game.
    public void ExitGame()
    {
        Application.Quit();
    }
}
