using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script in Idle, Trigger for light animation, shift lights, communitcation with mat
//---------------------------------------------------------------------------------------------

public class StartScriptSerial : MonoBehaviour
{

    public GameObject spot1;
    public GameObject spot2;
    public GameObject spot3;
    public GameObject rotator;
    public GameObject ball;
    public LeaveIdle leaveScript;
    public SerialScript serialScript;
    Rigidbody ballrb;

    public GameObject fuesseUI;
    public GameObject startUI;
    public Animator uiAnimation;

    Vector3 startPos1;
    public Vector3 endPos1;
    Quaternion end1;

    Vector3 startPos2;
    public Vector3 endPos2;
    Quaternion end2;


    Vector3 startPos3;
    public Vector3 endPos3;
    Quaternion end3;

    bool turnLightOn=false;
    float frameWert=150;
    bool shiftLights=false;

    int stateCounter=0;


    // Start is called before the first frame update
    void Start()
    {
        leaveScript.enabled=false;
        startPos1= spot1.transform.rotation.eulerAngles;
        startPos2= spot2.transform.rotation.eulerAngles;
        startPos3= spot3.transform.rotation.eulerAngles;
        ballrb=ball.GetComponent<Rigidbody>();
        ballrb.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        if(aufMatte() && stateCounter==0)
        {
            Debug.Log("State: " + stateCounter);
            stateCounter=1;
            turnLightOn=true;
            leaveScript.enabled=true;
            fuesseUI.SetActive(false);
            startUI.SetActive(true);
            uiAnimation.SetTrigger("pinballFull");
            ballrb.WakeUp();
            serialScript.sendState(1);
        }
        else if(Input.GetKeyDown("v") && stateCounter==1)
        {
            Debug.Log("State: " + stateCounter);
            serialScript.sendState(3);
            stateCounter=2;
            
        }
        else if(aufMatte() && stateCounter==2)
        {
            Debug.Log("State: " + stateCounter);
            serialScript.sendState(4);
            uiAnimation.SetBool("FadeOut",true);
            
        }
        



        if(turnLightOn)
        {
            spotAnimation();
        }

        if(shiftLights)
        {
            shiftLightAnimation();
        }

        if(spot3.transform.rotation== end3) //Wenn Licht Animation nach drauf gehen vorbei ist
        {
            shiftLights=true;
            turnLightOn=false;
        }

        

    }

    bool aufMatte() //Checkt ob man auf der Matte steht
    {
        if(Input.GetKey("s"))
        {
            return true;
        }
        else if(Input.GetKey("d"))
        {
            return true;
        }
        else if(Input.GetKey("f"))
        {
            return true;
        }else if(Input.GetKey("g"))
        {
            return true;
        }else if(Input.GetKey("h"))
        {
            return true;
        }else if(Input.GetKey("j"))
        {
            return true;
        }else if(Input.GetKey("k"))
        {
            return true;
        }else if(Input.GetKey("l"))
        {
            return true;
        }else if(Input.GetKey("t"))
        {
            return true;
        }
        else{
            return false;
        }
    }

    float checkWichKeyisPressed() //Gibt Verlagerung zurück
    {
        if(Input.GetKey("s"))
        {
            return -1f;
        }
        else if(Input.GetKey("d"))
        {
            return -0.75f;
        }
        else if(Input.GetKey("f"))
        {
            return -0.5f;
        }
        else if(Input.GetKey("g"))
        {
            return -0.25f;
        }
        else if(Input.GetKey("h"))
        {
            return 0.25f;
        }
        else if(Input.GetKey("j"))
        {
            return 0.5f;
        }
        else if(Input.GetKey("k"))
        {
            return 0.75f;
        }
        else if(Input.GetKey("l"))
        {
            return 1f;
        }
        else
        {
            return 0;
        }
    }

    void shiftLightAnimation()
    {
        float input= checkWichKeyisPressed() *-1*Time.deltaTime*frameWert/3;
        float diff= rotator.transform.position.x+11;

        if((diff<3 && input>0) || ( diff>-2 && input<0))
        {
        rotator.transform.position+= new Vector3(input*0.05f,0,0);
        }
    }

    void spotAnimation()
    {
            end1= Quaternion.Euler(endPos1);
            spot1.transform.rotation= Quaternion.Lerp(spot1.transform.rotation, end1,0.05f *Time.deltaTime*frameWert);

            end2= Quaternion.Euler(endPos2);
            spot2.transform.rotation= Quaternion.Lerp(spot2.transform.rotation, end2,0.05f*Time.deltaTime*frameWert );

            end3= Quaternion.Euler(endPos3);
            spot3.transform.rotation= Quaternion.Lerp(spot3.transform.rotation, end3,0.05f *Time.deltaTime*frameWert);
    }
}
