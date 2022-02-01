using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controlls for Onboarding 
//-----------------------------------------

public class SteuerungOnboarding : MonoBehaviour
{
    public GameObject balken;
    public GameObject ball;
    public GameObject indicator;
    public GameObject jumpIndicator;
    public GameObject timeIndicator;
    public GameObject camera;
    public GameObject canvas;
    public SerialScript serialScript;

    RectTransform timeIndicatorRect;
    RectTransform jumpIndicatorRect;

    OnboardingScript onboardingScript;

    public Sprite stripeLine;
    public Sprite loggedLine;
    Material freeMaterial;

    Color pink;
    Color transparent;
    Image indicatorImage;

    float speed = 0.01f;
    Rigidbody ballRb;

    float jumph = 5.5f;
    float jumpf = 0; 
    float jumpd = 0;
    float jumpdMax= 5.5f; 
    public float jumpfMax=2;

    Vector3 jump;
    bool ballOnPlatform = false;


    float dirTimer=5;
    float frameWert=200;

    bool start=false;
     bool isActive=false;
     bool jumpAllowed;
 

    Quaternion originRotation;
    void Start()
    {
        jump = new Vector3(0.2f, jumph, 0f);
        ballRb = ball.GetComponent<Rigidbody>();
        indicatorImage = indicator.GetComponent<Image>();
        pink=indicator.GetComponent<Image>().color;
        transparent= new Color(pink.a,pink.g,pink.b,0);
        //Debug.Log("pink" + pink);
        originRotation = Quaternion.Euler( 0, 0, 0);
        onboardingScript=canvas.GetComponent<OnboardingScript>();
        timeIndicatorRect= timeIndicator.GetComponent<RectTransform>();
        jumpIndicatorRect=jumpIndicator.GetComponent<RectTransform>();
        //serialScript.sendState(4);


    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("BallOnPlatform= " + ballOnPlatform+ " |start= " +start+ " |isActive= " + isActive);
        if(isActive)
        {
            if(!start)
            {
                indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, originRotation,  0.1f*Time.deltaTime*frameWert);
                jumpd=0;
            }
            jumpIndicatorRect.sizeDelta= new Vector2(jumpf/jumpfMax*2180,jumpIndicatorRect.sizeDelta.y);
            if(indicatorImage.color==transparent)
            {
                indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, originRotation,  0.1f*Time.deltaTime*frameWert);
                jumpd=0;
            }
            
            if(!ballOnPlatform)
            {
                indicatorImage.color=Color.Lerp(indicatorImage.color,transparent,0.1f*Time.deltaTime*frameWert);
                jumpIndicatorRect.sizeDelta= new Vector2(0,jumpIndicatorRect.sizeDelta.y);
            }

            if (ballOnPlatform &&start)
            {
                
                Vector3 screenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
                //Debug.Log("Mach ich jetzt");
                indicator.transform.position=new Vector3 (screenPos.x-4f,screenPos.y,screenPos.z);

                if(dirTimer<=0 && dirTimer >=-1)
                {
                    serialScript.sendState(2);
                    dirTimer=-2;
                }
                if(dirTimer>0)
                {
                    dirTimerFunc();
                    float inputRotation= checkWichKeyisPressed()*Time.deltaTime*frameWert*2f;
                    if((inputRotation<0 && jumpd>-jumpdMax) || (inputRotation>0 && jumpd<jumpdMax))
                    {
                        jumpd += 0.01f * inputRotation;
                    }

                    Quaternion target = Quaternion.Euler( 0, 0, (jumpd/jumpdMax) *-45);
                    indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, target,  0.1f*Time.deltaTime*frameWert);

                    Vector3 targetPos= new Vector3(0,0.113f, 0);
                }

                else{
                    indicatorImage.sprite=loggedLine;
                   // Debug.Log("sprite:  "+ indicatorImage.sprite);
                    makeJump();
                    onboardingScript.jumpNow();
                }
            }
        }
    }

    void OnTriggerEnter(Collider collidingObject)
    {
        if(isActive)
        {
        if (collidingObject.gameObject == ball)
        {
            serialScript.sendState(1);
            ballOnPlatform = true;
            dirTimer=5;
            jumpd=0;
            
            indicatorImage.color=pink;
            indicatorImage.sprite=stripeLine;
            
            onboardingScript.setOnArea(true);



        } 
        }
    }

   

    void OnTriggerExit(Collider collisionInfo)
    {
        if(isActive)
        {
        ballOnPlatform = false;
        
        indicatorImage.color=new Color(255,0,0,0);
        //blendOutRichtung();
        
        serialScript.sendState(4);
        }
        

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

    void makeJump()
    {
        if(jumpAllowed)
        {
        if(Input.GetKey("v"))
        {   
            if(jumpf<=jumpfMax)
            {
            jumpf+=0.05f*Time.deltaTime*frameWert;
            }
        }
        if(Input.GetKeyUp("v"))
        {
            jump = new Vector3(jumpd, jumph, 0f);
            ballRb.AddForce(jump * jumpf, ForceMode.Impulse);
            jumpf=0;
            onboardingScript.jumpedFunc();
            onboardingScript.setOnArea(false);

        }}
    }

    void dirTimerFunc()
    {
        dirTimer-= Time.deltaTime;
        timeIndicatorRect.sizeDelta= new Vector2(dirTimer*440, timeIndicatorRect.sizeDelta.y);
        //timeIndicator.transform.localScale= new Vector3(dirTimer*0.18f, timeIndicator.transform.localScale.y,timeIndicator.transform.localScale.z);

    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }


    public void resetJumpD(){
        jumpd=0;
        indicator.transform.rotation=  Quaternion.Euler(0,0,0);
    }

    public void setJumpD(float d)
    {
        jumpd=d;
        indicator.transform.rotation=  Quaternion.Euler(0,0,0);
        dirTimer=5;

    }

    public void jumpAfterLeaveBack()
    {
        jump = new Vector3(0,1,0);
        //ballRb.AddForce(jump * jumpf, ForceMode.Impulse);
        indicatorImage.sprite=stripeLine;
        dirTimer=5;
        jumpf=0;
        jumpIndicatorRect.sizeDelta= new Vector2(0,jumpIndicatorRect.sizeDelta.y);
        start=false;
        isActive=false;
        jumpAllowed=false;
        jumpf=0;
        resetJumpD();
    }

    public void blendOutRichtung()
    {
        indicatorImage.color=new Color(255,255,255,0);

    }

    
    public void blendInRichtung()
    {
         indicatorImage.color=new Color(255,255,255,255);
         

    }

    public void setBallonPlatform()
    {
        ballOnPlatform=true;
        dirTimer=5;
        start=true;
        jumpd=-0f;
    }

    public void setActive()
    {
        isActive=true;
    }

    public void setInActive()
    {
        isActive=false;
    }

    public void setJumpAllowed()
    {
        jumpAllowed=true;
    }
}
