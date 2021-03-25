using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharecterController : MonoBehaviour
{
    public Vector3 Velocity;
    public bool Grounded { get { return grounded; } }

    public Rigidbody Body { get { return body; } }

    private Rigidbody body;
    private Collider col;
    private Vector3 localBottem;
    private Ray downRay;
    private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        localBottem = transform.InverseTransformPoint(col.bounds.center - new Vector3(0, col.bounds.extents.y, 0));
        localBottem.y += 0.1f; // Skin

        downRay = new Ray(transform.position + localBottem, Vector3.down);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = (grounded) ? Color.black : Color.white;
        Gizmos.DrawSphere(transform.position + localBottem, 0.2f);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(downRay);
    }

    public void Move(Vector3 velocity)
    {
        Velocity = velocity;
        body.velocity = Vector3.zero;
    }

    public void SimpleMove(Vector3 velocity)
    {
        Velocity.x = velocity.x;
        Velocity.z = velocity.z;
    }

    public void Jump(float force)
    {
        body.velocity = new Vector3(body.velocity.x, 0, body.velocity.y);
        body.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        downRay.origin = transform.position + localBottem;
        if (Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y - 0.1f, col.bounds.center.z), col.bounds.extents.x - 0.1f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        body.velocity = new Vector3(Velocity.x, body.velocity.y + Velocity.y, Velocity.z);
    }
}
