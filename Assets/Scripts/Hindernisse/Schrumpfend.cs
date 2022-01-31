using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schrumpfend : MonoBehaviour
{
    public Vector3 minScale;
    Vector3 defaultScale;
    Vector3 defaultPos;
    public float shrinkTimes;
    float shrinkCounter=1;
    float frameWert=150;

    bool shrinking=false;

    Vector3[] shrinkSteps;


    // Start is called before the first frame update
    void Start()
    {
        defaultScale=transform.localScale;
        defaultPos=transform.position;
        int shrinks= (int)shrinkTimes;
        shrinkSteps= new Vector3[shrinks];

        for(int i=0; i<shrinks; i++)
        {
            shrinkSteps[i]= defaultScale - ((defaultScale-minScale)*(1/shrinkTimes)*(i+1));
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale= Vector3.Lerp(transform.localScale, makeRightScale(),0.1f*Time.deltaTime*frameWert/3);

        int shrinks= (int)shrinkTimes;
         for(int i=0; i<shrinks; i++)
        {
            shrinkSteps[i]= defaultScale - ((defaultScale-minScale)*(1/shrinkTimes)*(i+1));
        }

    }

    void OnCollisionEnter()
    {
        if(shrinkCounter<shrinkTimes)
        {
            shrinkCounter++;
            shrinking=true;
        }
        else
        {
            transform.position+= new Vector3(0,0,5);
        }
    }


    Vector3 makeRightScale()
    {
        if(shrinking)
        {
            return shrinkSteps[(int)shrinkCounter-1];
        }
        else{
            return transform.localScale;
        }
    }

    public void resetObject()
    {
        shrinkCounter=1;
        shrinking=false;
        transform.localScale=defaultScale;
        transform.position = defaultPos;
    }
}
