using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingLearning : MonoBehaviour
{

    public GameObject ball;
    Rigidbody ballRb;
    public GameObject indicator;
    public GameObject camera;
    public GameObject canvas;
    public SerialScript serialScript;
    public Animator onboardingUI;
    public OnboardingScript onboardingScript;
    

    float jumpd = 0;
    float jumpdMax= 5.5f; 
    bool onArea;
    bool rightDone=false;
    bool leftDone=false;
    float frameWert=200;

   
    int cntUp;


    // Start is called before the first frame update
    void Start()
    {
        ballRb=ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(onArea)
        {

            if(!leftDone )
            {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
            Debug.Log("target is " + screenPos);
            indicator.transform.position=new Vector3 (screenPos.x-4f,screenPos.y,screenPos.z);
            float inputRotation= checkWichKeyisPressed()*Time.deltaTime*frameWert*2f;
            Quaternion target = Quaternion.Euler( 0, 0, (jumpd/jumpdMax) *-45);
            indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, target,  0.1f*Time.deltaTime*frameWert);

            Vector3 targetPos= new Vector3(0,0.113f, 0); 

            if(Input.GetKey("v"))
            {
                cntUp++;
                if(cntUp>20)
                {
                    Debug.Log("SKIPPPPPPPPPPPPPPPP");
                }
            }
            else{
                cntUp=0;
            }
            if((inputRotation<0 && jumpd>-jumpdMax) || (inputRotation>0 && jumpd<jumpdMax))
            {
                jumpd += 0.01f * inputRotation;
            }
            if(jumpd>=jumpdMax-0.2f)
            {
                rightDone=true;
                onboardingUI.SetTrigger("ShiftRightDone");
            }
            if(jumpd<=-jumpdMax+0.2f && rightDone)
            {
                leftDone=true;
                onboardingUI.SetTrigger("ShiftLeftDone");


                //onboardingScript.shiftDone();
            }
            }
        }
        
            
    }

    void OnTriggerEnter(Collider collidingObject)
    {
        onArea=true;
        onboardingUI.SetTrigger("startOnboarding");
        serialScript.sendState(1);
    }


    float checkWichKeyisPressed()
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

    public void reset()
    {
        rightDone=false;
        leftDone=false;
        jumpd=0;
        onboardingUI.ResetTrigger("ShiftRightDone");
        onboardingUI.ResetTrigger("ShiftLeftDone");
  
    }
}
