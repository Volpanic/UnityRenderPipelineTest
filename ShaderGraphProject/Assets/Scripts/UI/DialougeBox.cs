using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialougeBox : MonoBehaviour // Handels fading in letters of the dialogue box.
{

    public TextMeshProUGUI DialougeText;
    public Image DialougePortraitImage;
    public float DelayBetweenLetter = 0.1f;
    public Transform DialougeDoneIndecator;

    private float timer = 0;
    private int maxCharecter = 1;
    private int messageCount = 0;

    [SerializeField]
    private DialougeSequence playingSequence;
    private byte CharecterAlpha = 0;
    private bool active = true;

    private void Start()
    {
        if(playingSequence != null)
        {
            DialougeText.text = playingSequence.DialougeLines[0].DialougeText;
            DialougePortraitImage.sprite = playingSequence.DialougeLines[0].SpeakerFace;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TypewritterEffect();
    }

    private void TypewritterEffect()
    {
        Color32[] colors = DialougeText.textInfo.meshInfo[0].colors32;

        // If Active update code
        if (active && colors != null && colors.Length > 0)
        {
            DialougeLineUpdate(colors);
        }
        else
        {
            DialougeLineFinished();
        }

        DialougeText.maxVisibleCharacters = maxCharecter;
    }

    private void DialougeLineUpdate(Color32[] colors)
    {
        //Turn off the done idecator 
        DialougeDoneIndecator.gameObject.SetActive(false);

        TMP_CharacterInfo current = DialougeText.textInfo.characterInfo[maxCharecter - 1];

        timer += Time.deltaTime;

        if (current.isVisible)
        {
            //Update Current Charecter alpha to allow fade in.
            float mod = timer / DelayBetweenLetter;
            CharecterAlpha = (byte)((Mathf.Clamp(timer / DelayBetweenLetter, 0f, 1f) * 255));
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

            if (maxCharecter >= DialougeText.textInfo.characterCount)
            {
                active = false;
                break;
            }

            //Set the old color
            current = DialougeText.textInfo.characterInfo[maxCharecter - 1];

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

    //Runs when the line of dialogue has concluded
    private void DialougeLineFinished()
    {
        //Turn on the done indicator 
        DialougeDoneIndecator.gameObject.SetActive(true);

        //Text should be done, so close if clicked
        if (Input.GetMouseButtonDown(0))
        {
            messageCount++;

            //Reset values and play dialouge
            active = true;
            maxCharecter = 1;
            CharecterAlpha = 0;
            timer = 0;

            if (messageCount > playingSequence.DialougeLines.Count - 1)
            {
                active = false;
                gameObject.SetActive(false);
            }
            else // Set new message info
            {
                //Set the text to be the first message
                DialougeText.text = playingSequence.DialougeLines[messageCount].DialougeText;
                DialougePortraitImage.sprite = playingSequence.DialougeLines[messageCount].SpeakerFace;
            }
        }
    }

    public void PlayDialouge(DialougeSequence playThis)
    {
        //Reset values and play dialouge
        active = true;
        maxCharecter = 1;
        CharecterAlpha = 0;
        timer = 0;
        messageCount = 0;

        //Set the text to be the first message
        DialougeText.text = playThis.DialougeLines[0].DialougeText;
        DialougePortraitImage.sprite = playThis.DialougeLines[0].SpeakerFace;
        playingSequence = playThis;

        //Make sure the box is active
        gameObject.SetActive(true);
    }
}
