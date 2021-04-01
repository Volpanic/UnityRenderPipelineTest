using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlayfield : MonoBehaviour
{
    private Vector3 respawnPos;
    private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");

        //Store startup varaibles to reset when player has left the play field
        respawnPos = obj.transform.position;
        rotation = obj.transform.eulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = respawnPos;
            other.gameObject.transform.eulerAngles = rotation;
        }
    }
}
