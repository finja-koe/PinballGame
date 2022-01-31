using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wachsend : MonoBehaviour
{
    public Vector3 maxScale;
    Vector3 defaultScale;
    Vector3 defaultPos;
    public float growTimes;
    float growCounter=1;
    float frameWert=150;

    bool growing=false;
    public float speed=0.1f;

    Vector3[] growSteps;


    // Start is called before the first frame update
    void Start()
    {
        defaultScale=transform.localScale;
        defaultPos=transform.position;

        int grows= (int)growTimes;
        growSteps= new Vector3[grows];

        for(int i=0; i<grows; i++)
        {
            growSteps[i]= defaultScale - ((defaultScale-maxScale)*(1/growTimes)*(i+1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale= Vector3.Lerp(transform.localScale, makeRightScale(),speed*Time.deltaTime*frameWert);

        int grows= (int)growTimes;
         for(int i=0; i<grows; i++)
        {
            growSteps[i]= defaultScale - ((defaultScale-maxScale)*(1/growTimes)*(i+1));
        }
    }

    void OnCollisionEnter()
    {
        if(growCounter<growTimes)
        {
            growCounter++;
            growing=true;
        }
    }

    Vector3 makeRightScale()
    {
        if(growing)
        {
            return growSteps[(int)growCounter-1];
        }
        else{
            return transform.localScale;
        }
    }

    public void resetObject()
    {
        growing=false;
        growCounter=1;
        transform.localScale=defaultScale;
        transform.position = defaultPos;
    }
}
