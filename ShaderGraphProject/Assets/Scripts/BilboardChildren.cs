using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilboardChildren : MonoBehaviour
{

    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Loops through all children and makes the rotate towards the camera.
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).LookAt(cameraTransform);
        }
    }
}
