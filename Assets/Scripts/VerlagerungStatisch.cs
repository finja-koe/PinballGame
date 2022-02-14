using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controll for Ball, shift weight, jump 
//------------------------------------------------------------------

public class VerlagerungStatisch : MonoBehaviour
{

    public GameObject balken;
    public GameObject ball;
    public GameObject indicator;
    public GameObject jumpIndicator;
    public GameObject timeIndicator;
    public GameObject camera;
    public Animator uiAnimation;
    public HeartScript heartScript;
    public SerialScript serialScript;
    public HintScript hintScript;

    RectTransform timeIndicatorRect;
    RectTransform jumpIndicatorRect;

    public Text jumpFeedback;
    public Sprite stripeLine;
    public Sprite loggedLine;
    Material freeMaterial;

    Color pink;
    Color transparent;
    Image indicatorImage;

    float speed = 0.01f;
    Rigidbody ballRb;

    float jumph = 5.5f;
    float jumpf = 0; //2.5f
    float jumpfTime=0;
    float jumpd = 0;
    float jumpdMax= 5.5f; //vorher 4
    public float jumpfMax=2;

    Vector3 jump;
    bool ballOnPlatform = false;


    float dirTimer;
    float dirTimerMax=4;
    float frameWert=200;

    float stuckTimer=0;


    float timerLine;
    Quaternion originRotation;
    // Start is called before the first frame update
    void Start()
    {
        jump = new Vector3(0.2f, jumph, 0f);
        ballRb = ball.GetComponent<Rigidbody>();
        indicatorImage = indicator.GetComponent<Image>();
        pink= new Color(255,255,255,255);
        transparent= new Color(pink.a,pink.g,pink.b,0);
        Debug.Log("pink" + pink);
        originRotation = Quaternion.Euler( 0, 0, 0);
        timeIndicatorRect= timeIndicator.GetComponent<RectTransform>();
        jumpIndicatorRect=jumpIndicator.GetComponent<RectTransform>();
        //indicator.transform.position=new Vector3 (536.4f,218.3f,4.9f);
        

    }

    // Update is called once per frame
    void Update()
    {
        checkIfStuck();
        // Setzt Größe von JumpIndicator
        //jumpIndicator.transform.localScale= new Vector3(jumpf/jumpfMax,jumpIndicator.transform.localScale.y,jumpIndicator.transform.localScale.z);
        jumpIndicatorRect.sizeDelta= new Vector2(jumpf/jumpfMax*2180,jumpIndicatorRect.sizeDelta.y);
        
        //Macht DirIndicator gerade, nachdem er ausgeblendet wurde
        if(indicatorImage.color==transparent)
        {
            indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, originRotation,  0.1f*Time.deltaTime*frameWert);
            jumpd=0;
        }
        
        // Blendet DirIndicator aus, nachdem Ball Platform verlassen hat
        if(!ballOnPlatform)
        {
            indicatorImage.color=Color.Lerp(indicatorImage.color,transparent,1f);
        }

        //Ball ist unten
        if (ballOnPlatform)
        {
            // Setzt DirIndicator aus Canvas auf richtige Position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
            indicator.transform.position=new Vector3 (screenPos.x-4f,screenPos.y,screenPos.z);

            if(dirTimer<=0 && dirTimer>=-1)
            {
                serialScript.sendState(2);
                dirTimer=-2;
                 hintScript.startHint();
                
            }
            //Wenn Richtung eingestellt wird
            if(dirTimer>0)
            {
                dirTimerFunc();

                //Setzt Rotation von dirIndicator
                float inputRotation= checkWichKeyisPressed()*Time.deltaTime*frameWert*2f;
                if((inputRotation<0 && jumpd>-jumpdMax) || (inputRotation>0 && jumpd<jumpdMax))
                {
                    jumpd += 0.01f* inputRotation  ;
                }

                Quaternion target = Quaternion.Euler( 0, 0, (jumpd/jumpdMax) *-45);
                indicator.transform.rotation = Quaternion.Slerp(indicator.transform.rotation, target,  0.1f*Time.deltaTime*frameWert);
                Vector3 targetPos= new Vector3(0,0.113f, 0);

            }

            // Wenn VerlagerungZeit abgelaufen 
            else{
                indicatorImage.sprite=loggedLine;
                makeJump();
               
            }
        }

        if(Input.GetKeyDown("e"))
        {
            makeLittleJump();
        }
    }

    //Checkt ob Ball auf Platform ist
    void OnTriggerEnter(Collider collidingObject)
    {
        if (collidingObject.gameObject == ball)
        {
            
        }

        serialScript.sendState(1);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == ball)
        {
            timerLine+= Time.deltaTime;
            if(timerLine>0.5f && timerLine<2)
            {
                ballOnPlatform = true;
                dirTimer=dirTimerMax;
                jumpd=0;
                indicatorImage.color=pink;
                indicatorImage.sprite=stripeLine;
                timerLine=3;
            }
        }
    }

    //Checkt ob Ball Platform verlassen hat
    void OnTriggerExit(Collider collisionInfo)
    {
        ballOnPlatform = false;
        timerLine=0;
        serialScript.sendState(4);
    }

    //Gibt float Werte zurück für Verlagerung
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

    // Merkt sich wie doll gesprungen und schießt Ball weg
    void makeJump()
    {

        if(Input.GetKey("v"))
        {   
            if(jumpf<=jumpfMax)
            {
            jumpf+=13f*Time.deltaTime;  //Hier Wert verändern damit Jump stärker wird
            jumpfTime+= Time.deltaTime;
            }
            
        }
        if(Input.GetKeyUp("v"))
        {
            jumpFeedback.text=getJumpFeedBack(jumpf, jumpfMax);
            uiAnimation.SetTrigger("jumpFeedback");
            jump = new Vector3(jumpd, jumph, 0f);
            ballRb.AddForce(jump * jumpf, ForceMode.Impulse);
            jumpf=0;
            Debug.Log("Zeit für JumpMax: " + jumpfTime);
            jumpfTime=0;
            hintScript.leaveHint();
            uiAnimation.SetBool("JumpHint", false);
        }
    }

    // Passt Zeit und Größe des TimeIndicators an
    void dirTimerFunc()
    {
        dirTimer-= Time.deltaTime;
        timeIndicatorRect.sizeDelta= new Vector2(dirTimer*2200/dirTimerMax, timeIndicatorRect.sizeDelta.y); //2200 = timeIndicator.maxWidth

    }

    //Für Kamera rotation -> Nicht mehr mit drin (Übersetzt BallPos in Kamera Rotation)
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }


    //Reset JumpD nach Sprung wird von TeleportScript aufgerufen
    public void resetJumpD(){
        jumpd=0;
        indicator.transform.rotation=  Quaternion.Euler(0,0,0);

    }

    string getJumpFeedBack(float jumpfCurrent, float maxForce)
    {
        float drittleOfMax= maxForce/3;
        if(jumpfCurrent<= maxForce*0.4f && jumpfCurrent>0)
        {
            return "good";
        }
        else if(jumpfCurrent<= maxForce*0.8f)
        {
            return "nice";
        }
        else if(jumpfCurrent> maxForce*0.8f)
        {
            return "perfect";
        }
        else{
            return "nothing";
        }
    }

    public void blendOutRichtung()
    {
         indicatorImage.color=transparent;

    }

    
    public void blendInRichtung()
    {
         indicatorImage.color=new Color(255,255,255,255);;

    }

    public void setJumpfZero()
    {
        jumpf=0;
    }

    public void resetDirTimer()
    {
        dirTimer=dirTimerMax;
        timeIndicatorRect.sizeDelta= new Vector2(dirTimer*2200/dirTimerMax, timeIndicatorRect.sizeDelta.y);
    
        resetJumpD();
    }

    public void rightPosIndicator()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(ball.transform.position);
        indicator.transform.position=screenPos;
    }

    void makeLittleJump()
    {
        Vector3 jump= new Vector3(0.2f,2,0);
        ballRb.AddForce(jump,ForceMode.Impulse);
    }

    void checkIfStuck()
    {
        if(ballRb.IsSleeping())
        {
            stuckTimer+= Time.deltaTime;
        }
        else{
            stuckTimer=0;
        }
        if(stuckTimer>=5 && !ballOnPlatform)
        {
            makeLittleJump();
        }
        

    }

    public float getJumpD()
    {return jumpd;}

    public void jumpAfterLeaveBack()
    {
        jump = new Vector3(0,1,0);
        indicatorImage.sprite=stripeLine;
        dirTimer=dirTimerMax;
        jumpf=0;
        resetJumpD();
    }

    
        
}
