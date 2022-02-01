using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


// Let ball and shell blink bright at beginning of any Level
//---------------------------------------------------------------------------------


public class BlinkingScript : MonoBehaviour
{
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color colorNormal;
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)] public Color colorBright;
    Color currentColorBall;
    Color currentColorShell;
    public Material ballMaterial;
    public Material shellMaterial;
    public TeleportScript teleportScript;
    bool blinking=false;
    bool lerpToMax=true;
    bool doTimer=false;
    bool doBlink=true;
    float timer=0;
    
    float framewert = 150;

    public Volume volume;
    Bloom bloom;
    float bloomMax=3f;
    

    // Start is called before the first frame update
    void Start()
    {
        currentColorBall=colorNormal;
        currentColorShell=colorNormal;        
    }

    // Update is called once per frame
    void Update()
    {
       if(blinking)
        {
            newBlink();
        }
         if(Input.GetKey("w"))
        {
            blinking=true;

        }

       
    }

    void newBlink()
    {
        Color nextColorBall;
        Color nextColorShell;
        currentColorBall=ballMaterial.GetColor("_Color");
        currentColorShell=shellMaterial.GetColor("_Color");
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
