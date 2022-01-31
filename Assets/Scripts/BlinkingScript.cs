using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


// Let ball and shell blink bright at beginning of any Level
//---------------------------------------------------------------------------------


public class BlinkingScript : MonoBehaviour
{
    //public ColorUsageAttribute(bool hdr) ballColor;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color colorNormal;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color colorBright;
    Color currentColorBall;
    Color currentColorShell;
    public Material ballMaterial;
    public Material shellMaterial;
    public TeleportScript teleportScript;
    //float intensity=0.58f;
    //public float max=10f;
    //public float speed=0.05f;
    //float direction=1;
    bool blinking=false;
    bool lerpToMax=true;
    //Color brightColor= new Color(350*2,91*2,75*2,100*2);
    bool doTimer=false;
    bool doBlink=true;
    float timer=0;
    
    float framewert = 150;
     //Renderer rend;

    public Volume volume;
    Bloom bloom;
    float bloomMax=3f;
    

    // Start is called before the first frame update
    void Start()
    {
        //rend = GetComponent<Renderer> ();
        currentColorBall=colorNormal;
        currentColorShell=colorNormal;
        if(volume.profile.TryGet<Bloom>(out bloom))
        {}
        
        
           
        
        
    }

    // Update is called once per frame
    void Update()
    {
       if(blinking)
        {
            newBlink();
            /*timer-= Time.deltaTime;
            ballMaterial.SetColor("_Color",colorBright);
            //bloom.intensity.value=2;
            bloom.intensity.value= Mathf.PingPong(Time.time*4 ,2);
            if(timer<0)
            {
                ballMaterial.SetColor("_Color",colorNormal);
                bloom.intensity.value=0;
                timer=1;
                blinking=false;
            }*/
        }
         if(Input.GetKey("w"))
        {
            blinking=true;

        }

       
       /* if(Input.GetKey("w"))
        {
            ballMaterial.SetColor("_Color",colorBright);
            //bloom.intensity.value=2;
            bloom.intensity.value= Mathf.PingPong(Time.time*5 ,2);

        }
        else{
             ballMaterial.SetColor("_Color",colorNormal);
             bloom.intensity.value=0;
            

        }
        /*if(Input.GetKey("w"))
        {
            currentColor= Color.Lerp(currentColor, colorBright,0.01f);
            ballMaterial.SetColor("_Color",currentColor);
        }
        if(Input.GetKey("e"))
        {
             currentColor= Color.Lerp(currentColor, colorNormal,0.01f);
            ballMaterial.SetColor("_Color",currentColor);
        }*/

       
    }

/*
    void makeBlink()
    {
        intensity+=speed*direction;
        if(intensity>max)
        {
            direction*=-1;
        }
        if(intensity<0.58f)
        {
            direction*=-1;
            blinking=false;
        }
         ballMaterial.SetFloat("_Ambient",intensity);
    }

    void makeBlinkEmission()
    {
        Debug.Log("Timer:" + timer);
        ballMaterial.SetColor("_Color",colorBright);
        timer-= Time.deltaTime;
        if(timer<0.05f)
        {
            ballMaterial.SetColor("_Color",colorNormal);
            timer=1;
            blinking=false;
        }
    }

      void makeBlinkEmissionLerp()
    {
        currentColor= Color.Lerp(currentColor, colorBright,0.01f);
        ballMaterial.SetColor("_Color",currentColor);
        /*
        timer-= Time.deltaTime;
        if(timer<0.05f)
        {
            ballMaterial.SetColor("_Color",colorNormal);
            timer=1;
            blinking=false;
        }
    } */

    void newBlink()
    {
        Color nextColorBall;
        Color nextColorShell;
        currentColorBall=ballMaterial.GetColor("_Color");
        currentColorShell=shellMaterial.GetColor("_Color");
        //ballMaterial.SetColor("_Color",colorBright);
        if(doBlink){
            if(lerpToMax)
            {
                bloom.intensity.value=Mathf.Lerp(bloom.intensity.value,bloomMax,0.05f *Time.deltaTime*framewert);
                nextColorBall= Color.Lerp(currentColorBall,colorBright,0.05f*Time.deltaTime*framewert);
                nextColorShell= Color.Lerp(currentColorShell,colorBright,0.05f*Time.deltaTime*framewert);

                ballMaterial.SetColor("_Color",nextColorBall);
                shellMaterial.SetColor("_Color",nextColorShell);

                if(bloomMax-bloom.intensity.value<=0.06)
                {
                    lerpToMax=false;
                }
            }
            if(!lerpToMax)
            {
                bloom.intensity.value=Mathf.Lerp(bloom.intensity.value,0,0.05f*Time.deltaTime*framewert);
                nextColorBall= Color.Lerp(currentColorBall,colorNormal,0.05f*Time.deltaTime*framewert);
                nextColorShell= Color.Lerp(currentColorShell,colorNormal,0.05f*Time.deltaTime*framewert);

                ballMaterial.SetColor("_Color",nextColorBall);
                shellMaterial.SetColor("_Color",nextColorShell);

                if(bloom.intensity.value<=0.06)
                {
                    bloom.intensity.value=0;
                    ballMaterial.SetColor("_Color",colorNormal);
                    shellMaterial.SetColor("_Color",colorNormal);

                    lerpToMax=true;
                    doBlink=false;
                    doTimer=true;
                }
            }
        }
        
        if(doTimer)
        {
            timer-= Time.deltaTime;
            if(timer<0)
            {
                doTimer=false;
                doBlink=true;
                blinking=false;
                timer=1;
                teleportScript.makeNextState();

            }
        }
    }

    public void startBlink()
    {
        blinking=true;
    }
}
