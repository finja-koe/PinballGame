using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnUndAus : MonoBehaviour
{
    public float timeOn;
    public float timeOff;
    public bool scale;
    public bool move;
    float timer;

    bool on=true;
    bool off=false;
    bool shrink=false;
    bool grow=false;
    public float switchSpeed=0.2f;

    private Vector3 velocity = Vector3.zero;
    Vector3 originScale;
    Vector3 originPos;
    Vector3 backPos;
    Vector3 zero;

    float frameWert=150;


    // Start is called before the first frame update
    void Start()
    {
        timer=timeOn;
        zero= new Vector3(0,0,0);
        originScale=transform.localScale;
        originPos=transform.position;
        backPos= new Vector3(originPos.x,originPos.y,(-originPos.z+0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(on || off)
        {
            timer-=Time.deltaTime;
        }

        if(on && timer<=0)
        {
            shrink=true;
            on=false;
        }
        else if(shrink)
        {
            if(scale)
            {
                transform.localScale=Vector3.SmoothDamp( transform.localScale, zero,ref velocity, switchSpeed);
                if(transform.localScale==zero)
                {
                    shrink=false;
                    off=true;
                    timer=timeOff;
                }
            }
            else if(move)
            {
                transform.position=Vector3.SmoothDamp( transform.position, backPos,ref velocity, switchSpeed);
                if(Vector3.Distance(transform.position, backPos) <= 0.01f)
                {
                    shrink=false;
                    off=true;
                    timer=timeOff;
                }
            }
        }
        else if(off && timer<=0)
        {
            grow=true;
            off=false;
        }
        else if(grow)
        {
            if(scale)
            {
                transform.localScale=Vector3.SmoothDamp(transform.localScale, originScale, ref velocity, switchSpeed);
                if(transform.localScale==originScale)
                {
                    grow=false;
                    on=true;
                    timer=timeOn;
                }
            }
            else if(move)
            {
                transform.position=Vector3.SmoothDamp(transform.position, originPos, ref velocity, switchSpeed);
                if(Vector3.Distance(transform.position, originPos) <= 0.01f)
                {
                    grow=false;
                    on=true;
                    timer=timeOn;
                }
            }

        }
    }
}
