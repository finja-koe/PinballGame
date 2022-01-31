using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schalter : MonoBehaviour
{

    bool pressButton=false;
    bool goBack=false;
    bool stateOn=false;

    Vector3 originScale;
    public Vector3 pressedScale;
    private Vector3 velocity = Vector3.zero;

    public GameObject switchObj;
    public float goBackZ;
    public float goForwardZ;
    public float speed=0.01f;


    bool letObjSwitch;

    bool timeDelayOk=true;
    float currentDelay;
    float timeDelay=1;
    bool startTimer=false;

    float frameWert=150;

    // Start is called before the first frame update
    void Start()
    {
        originScale=transform.localScale;
        //pressedScale=new Vector3(originScale.x,0.01f,originScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            timeDelayFunc();
        }
        pressButtonFunc();

        
            if(letObjSwitch)
            {
                if(stateOn)
                {
                    switchObj.transform.position+= new Vector3(0,0,speed*Time.deltaTime*frameWert);
                    if(switchObj.transform.position.z>goBackZ)
                    {
                        letObjSwitch=false;
                    }
                }
                else{
                    switchObj.transform.position+= new Vector3(0,0,-speed*Time.deltaTime*frameWert);
                    if(switchObj.transform.position.z<goForwardZ)
                    {
                        letObjSwitch=false;
                    }

                }
            }
        
    }

    void OnCollisionEnter()
    {
        pressButton=true;
        letObjSwitch=true;
        if(timeDelayOk)
        {stateOn=!stateOn;
        startTimer=true;}

    }

    void pressButtonFunc()
    {
        if(pressButton)
        {
            transform.localScale=Vector3.SmoothDamp( transform.localScale, pressedScale,ref velocity, 0.05f);
            if(transform.localScale==pressedScale)
            {
                pressButton=false;
                goBack=true;

            }
        }
        else if(goBack)
            {
                transform.localScale=Vector3.SmoothDamp( transform.localScale, originScale,ref velocity, 0.05f);
                if(transform.localScale==originScale)
                {
                    goBack=false;
                }
            }

    }

    public void resetObject()
    {
        stateOn=false;
        switchObj.transform.position= new Vector3(switchObj.transform.position.x,switchObj.transform.position.y,goForwardZ);
    }

    void timeDelayFunc()
    {
        if(currentDelay<timeDelay)
        {        
            currentDelay+= Time.deltaTime;
            timeDelayOk=false;
        }
        else{
            currentDelay=0;
            timeDelayOk=true;
            startTimer=false;
        }
    }

    
}
