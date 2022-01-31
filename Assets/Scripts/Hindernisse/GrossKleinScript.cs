using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrossKleinScript : MonoBehaviour
{
    public Vector3 maxScale;
    public Vector3 minScale;
    public float speed;
    float frameWert=150;

    private Vector3 velocity= Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.localScale, minScale) <= 0.01f)
        {
                Vector3 remember = maxScale;
                maxScale = minScale;
                minScale = remember;
        }
         transform.localScale = Vector3.SmoothDamp(transform.localScale, minScale, ref velocity, speed);
    }
}
