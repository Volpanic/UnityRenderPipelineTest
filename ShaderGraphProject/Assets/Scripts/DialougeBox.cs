using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialougeBox : MonoBehaviour // Handels fading in letters of the dialouge box.
{

    public TextMeshProUGUI DialougeText;
    public float DelayBetweenLetter = 0.1f;
    public Transform DialougeDoneIndecator;

    private float timer = 0;
    private int maxCharecter = 1;

    private byte CharecterAlpha = 0;

    private bool active = true;

    // Update is called once per frame
    void Update()
    {

        //DialougeText.mesh.GetColors(colors);
        Color32[] colors = DialougeText.textInfo.meshInfo[0].colors32;
        Vector3[] verticies = DialougeText.textInfo.meshInfo[0].vertices;

        // If Active update code
        if (active && colors != null && colors.Length > 0)
        {
            //Turn off the done idecator 
            DialougeDoneIndecator.gameObject.SetActive(false);

            TMP_CharacterInfo current = DialougeText.textInfo.characterInfo[maxCharecter-1];

            timer += Time.deltaTime;

            if (current.isVisible)
            {
                //Update Current Charecter alpha to allow fade in.
                float mod = timer / DelayBetweenLetter;
                CharecterAlpha = (byte)((Mathf.Clamp(timer / DelayBetweenLetter,0f,1f) * 255));
                colors[current.vertexIndex + 0].a = CharecterAlpha;
                colors[current.vertexIndex + 1].a = CharecterAlpha;
                colors[current.vertexIndex + 2].a = CharecterAlpha;
                colors[current.vertexIndex + 3].a = CharecterAlpha;

            }


            while (timer > DelayBetweenLetter)
            {
                timer -= DelayBetweenLetter;

                //Hard set the current charecter alpha

                if (current.isVisible)
                {
                    byte fCharecterAlpha = 255;
                    colors[current.vertexIndex + 0].a = fCharecterAlpha;
                    colors[current.vertexIndex + 1].a = fCharecterAlpha;
                    colors[current.vertexIndex + 2].a = fCharecterAlpha;
                    colors[current.vertexIndex + 3].a = fCharecterAlpha;
                }


                //Increment Charecter
                maxCharecter++;

                if(maxCharecter >= DialougeText.textInfo.characterCount)
                {
                    active = false;
                    break;
                }

                //Set the old color
                current = DialougeText.textInfo.characterInfo[maxCharecter-1];
                
                if (current.isVisible)
                {
                    byte nCharecterAlpha = 0;
                    colors[current.vertexIndex + 0].a = nCharecterAlpha;
                    colors[current.vertexIndex + 1].a = nCharecterAlpha;
                    colors[current.vertexIndex + 2].a = nCharecterAlpha;
                    colors[current.vertexIndex + 3].a = nCharecterAlpha;
                }

                if (DelayBetweenLetter <= 0) break;
            }

            DialougeText.UpdateVertexData();
        }
        else
        {
            //Turn on the done idecator 
            DialougeDoneIndecator.gameObject.SetActive(true);

            //Text should be done, so close if clicked
            if(Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
            }
        }

       DialougeText.maxVisibleCharacters = maxCharecter;
    }
}
