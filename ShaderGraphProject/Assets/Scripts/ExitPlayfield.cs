using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Causes the player to respawn when leaving the playfield
// The playfield is marked by child box colliders set as triggers

public class ExitPlayfield : MonoBehaviour
{
    public UnityEvent OnExitPlayfield;

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
            // The default charecter controller doesn't allow you
            // to manually set the players position, so disable it briefly
            CharacterController controller = other.GetComponent<CharacterController>();

            controller.enabled = false;
            other.gameObject.transform.position = respawnPos;
            other.gameObject.transform.eulerAngles = rotation;
            controller.enabled = true;

            OnExitPlayfield.Invoke();
        }
    }
}
