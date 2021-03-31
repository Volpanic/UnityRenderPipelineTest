using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougePortrait : MonoBehaviour
{
    public float Speed = 1;

    private Vector2  originalHeight;
    private Vector2 movingRect = Vector2.zero;
    private float timer = 0;

    private void OnEnable()
    {
        //Reset it so the effect can run again
        timer = 0;
        //((RectTransform)transform).sizeDelta = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalHeight = ((RectTransform)transform).sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Make the dialouge portrait's height expand unitl it reches it's original
        ((RectTransform)transform).sizeDelta = Vector2.Lerp(new Vector2(originalHeight.x,0), originalHeight, timer / Speed);
    }
}
