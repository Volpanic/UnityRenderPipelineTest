using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquashAnStretch : MonoBehaviour
{
    public float ReformSpeed = 1f;

    [Range(0,0.1f)]
    public float SettleGap = 0.09f;

    public Vector3 SquishAmount
    {
        get
        {
            return squishAmount;
        }

        set
        {
            squishAmount = value;
            this.enabled = true;
        }
    }

    private Vector3 squishAmount = Vector3.one;

    // Update is called once per frame
    void Update()
    {
        squishAmount = Vector3.Lerp(squishAmount,Vector3.one,ReformSpeed * Time.deltaTime);
        transform.localScale = squishAmount;

        if (Mathf.Abs(squishAmount.magnitude - 1) < SettleGap)
        {
            //transform.localScale = Vector3.one;
            //this.enabled = false;
        }
    }
}
