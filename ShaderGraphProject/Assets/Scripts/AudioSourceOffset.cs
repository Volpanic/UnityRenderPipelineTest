using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// On startup sets the audio clip played by an audiosource
// to a specified offset.

public class AudioSourceOffset : MonoBehaviour
{
    //The normalized offset of the audio source
    [Range(0,1)]
    public float Offset = 0;

    void Start()
    {
        AudioSource As = GetComponent<AudioSource>();
        if(As != null)
        {
            As.time = As.clip.length * Offset;
        }
        else
        {
            //Display an error in the console if we can't find an AudioSource
            Debug.LogWarning("Audio source offset placed on object without an AudioSource. " + gameObject.name + ".");
        }
    }
}
