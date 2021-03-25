using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Transform Pivot;
    public Vector3 Offset;

    [Range(0.1f,1f)]
    public float Sensitivity = 1;

    public float RotateSpeed;
    public float RotateRadius;
    public float MinViewAngle = -45f;
    public float MaxViewAngle = 45f;

    // Start is called before the first frame update
    void Start()
    {
        if (Offset != null && Offset == Vector3.zero)
        {
            Offset = Target.position - transform.position;
        }

        Pivot.transform.position = Target.position + (Target.right * RotateRadius);
        Pivot.transform.parent = Target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLockState();

        //Get the xpos of the mouse
        float mH = Input.GetAxis("Mouse X") * RotateSpeed;
        Target.Rotate(0, mH * Sensitivity, 0);

        // Y 
        float mV = Input.GetAxis("Mouse Y") * RotateSpeed;
        Pivot.Rotate(-mV * Sensitivity, 0, 0);

        //Limit Camera
        if (Pivot.rotation.eulerAngles.x > MaxViewAngle && Pivot.rotation.eulerAngles.x < 180f)
        {
            Pivot.rotation = Quaternion.Euler(MaxViewAngle, 0, 0);
        }

        if (Pivot.rotation.eulerAngles.x > 180f && Pivot.rotation.eulerAngles.x < 360f + MinViewAngle)
        {
            Pivot.rotation = Quaternion.Euler(360f + MinViewAngle, 0, 0);
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float desYAngle = Target.eulerAngles.y;
            float desXAngle = Pivot.eulerAngles.x;
            Quaternion rot = Quaternion.Euler(desXAngle, desYAngle, 0);
            transform.position = Target.position - (rot * Offset);
        }

        if (transform.position.y < Target.position.y - 2.5f)
        {
            transform.position = new Vector3(transform.position.x, Target.position.y - 2.5f, transform.position.z);
        }

        transform.LookAt(Target.position +  ((Target.right + Target.up).normalized * RotateRadius));
    }

    public void HandleLockState()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
