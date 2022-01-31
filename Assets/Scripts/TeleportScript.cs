using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script for check if Level done/Time Over/, moving Camera, teleporting ball, connection between many scripts
//--------------------------------------------------------------------------------------------------------------

public class TeleportScript : MonoBehaviour
{

    public  GameObject[] teleportEntrys= new GameObject[5];
    public  GameObject[] teleportExits= new GameObject[5];
    
    public GameObject camera;
    public GameObject indicators;
 
    public float[] cameraPosY= new float[5];
    public float[] indicatorPosY= new float[5];

    public GameObject area;
    VerlagerungStatisch verlagerungScript;
    public BlinkingScript blinkingScript;
    public SerialScript serialScript;

    public GameObject rectUI;
    HeartScript rectScript;

    int currentEntry=-1;
    bool allowMoveCamera=false;
    bool allowTimer=false;
    bool allowTeleportBall= false;
    int transitionStats=0;
    public GameObject roehre;
    bool isOnGround=false;


    Rigidbody ballRb;

    float timerToTransition=1;
    float framewert=150;
    public Animator uiAnimation; 


    //Alles für LevelAnzeige
    //public Text[] levelNumbers= new Text[5];
    public Image[] levelKasten= new Image[5];
    public Color pink;
    public Color darkBlue;
    //Vector2[] targetPosKasten= new Vector2[5];
    //Vector2 targetSizeKasten;
    bool doLevelAnzeige=false;

    // Start is called before the first frame update
    void Start()
    {
        
        verlagerungScript= area.GetComponent<VerlagerungStatisch>();
        rectScript= rectUI.GetComponent<HeartScript>();
        ballRb=GetComponent<Rigidbody>();

        /*for(int i=0; i<5; i++)
        {
            targetPosKasten[i]= levelKasten[i].anchoredPosition + new Vector2(27,0);
            //new Vector2(originPos.x+27,originPos.y);
        }
        targetSizeKasten= new Vector2(52,levelKasten[0].sizeDelta.y); */
    }

    // Update is called once per frame
    void Update()
    {
        if(doLevelAnzeige)
        {
            levelAnzeigeAnimation();
        }
         //Startet delay vor Bewegung
        if(allowTimer)
        {
            startTimer();
        }

        //Bewegt Kamera
        if(allowMoveCamera)
        { 
            moveCamera();
        }

        //Teleportiert Ball + Röhre
        if(allowTeleportBall)
        {
            teleportBall(currentEntry+1);
        }
    }

    //Checkt ob Ball in einer Schale ist, startet passendes UI Element 
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject==area)
        {
            isOnGround=true;
            
            
        }

        if(collider.gameObject.tag=="TeleportEntry")
        {
            Debug.Log(" returnIsTimerOver= " + rectScript.returnIsTimeOver());
            if(!rectScript.returnIsTimeOver())
            {
                currentEntry= checkWhichEntry(collider.gameObject);
                if(currentEntry<4)
                {
                    int nxtLevel= (currentEntry+1)%5;
                    rectScript.doWellDoneFunc(nxtLevel+1); //-> HeartScript
                    serialScript.sendState(5);
                }
                else{
                    rectScript.doWinFunc(); //-> HeartScript
                    serialScript.sendState(7);
                }
                indicators.transform.position= new Vector3(indicators.transform.position.x,indicatorPosY[(currentEntry+1)%5],indicators.transform.position.z);
            }  
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject==area)
        {
            isOnGround=false;
           
        }
    }

    //Gibt Entry zurück
    int checkWhichEntry(GameObject collider)
    {
         for (int i = 0; i < 5; i++)
         {
            if(collider==teleportEntrys[i])
            {
                return i;
            } 
         }
         return 5;
    }

    //Teleportiert Ball
    void teleportBall(int level)
    {
        if(transitionStats==0) //Röhre und Ball positionieren
        {
            //Röhre an richtige Stelle bringen
            roehre.transform.position=teleportExits[level].transform.position;
            roehre.transform.position= new Vector3(roehre.transform.position.x,roehre.transform.position.y,0.317f);
            //Ball an richtige Stelle bringen + schlafen
            transform.position= teleportExits[level].transform.position;
            transform.position= new Vector3(transform.position.x,transform.position.y,0.1f);
            ballRb.Sleep();

            //rectScript.resetTime();

            transitionStats++;
        }
        else if( transitionStats==1)// Röhre ins Spiel fahren + Ball losrollen lassen
        {
            Vector3 taregtPosRoe= new Vector3(roehre.transform.position.x,roehre.transform.position.y,0.141f);
            roehre.transform.position = Vector3.Slerp(roehre.transform.position,taregtPosRoe,0.1f*Time.deltaTime*framewert);
            if(roehre.transform.position.z-0.141f<=0.01f)
            {
                transitionStats++;
                ballRb.WakeUp();
                ballRb.constraints = RigidbodyConstraints.None;
            }
        }
        else if(transitionStats==2) // Ball am Boden auf richtige zPos bringen, Ball aufblinken lassen
        {
            if(isOnGround)
            {
                Vector3 targetPosBall= new Vector3(transform.position.x,transform.position.y,-0.17f);
                transform.position= Vector3.Slerp(transform.position,targetPosBall,0.9f*Time.deltaTime*framewert);
                if(transform.position.z+0.17<=0.001f)
                {
                     ballRb.constraints = RigidbodyConstraints.FreezePositionZ;
                     transitionStats++;
                     
                }
            }
        }
        else if(transitionStats==3) //Röhre zurück fahren, blink starten
        {
            Vector3 taregtPosRoeBack= new Vector3(roehre.transform.position.x,roehre.transform.position.y,0.317f);
            roehre.transform.position = Vector3.Slerp(roehre.transform.position,taregtPosRoeBack,0.05f*Time.deltaTime*framewert);
            if(0.317f-roehre.transform.position.z<=0.01f)
            {
                blinkingScript.startBlink();
                //transitionStats=0;
                //allowTeleportBall=false;
            }
            
        }
        else if(transitionStats==4) //transition beenden
        {
            rectScript.doTime();
            transitionStats=0;
            verlagerungScript.enabled=true;
            //verlagerungScript.rightPosIndicator();
            allowTeleportBall=false;
        }
        

        /*transform.position= teleportExits[(currentEntry+1)%5].transform.position;
        rectScript.resetTime(); //-> HeartScript
        rectScript.doTime(); //-> HeartScript*/
        //camera.transform.position= new Vector3(camera.transform.position.x, cameraPosY[(currentEntry+1)%5], camera.transform.position.z);
    }

    //Lässt Ball bei Exit raus wenn retry
    public void retryBallPos()
    {   
        Debug.Log("Current" + currentEntry); 
        //transform.position= teleportExits[currentEntry+1].transform.position;
        allowTeleportBall=true;
    }

    //Animiert Kamera
    void moveCamera()
    {
        if(Mathf.Abs(cameraPosY[(currentEntry+1)%5] - camera.transform.position.y) <= 0.01f)
        {
            rectScript.resetTime();
            //uiAnimation.SetBool("transition", false); //-> Blendet UI Elemente oben wieder ein
            //teleportBall();
            allowTeleportBall=true;
            allowMoveCamera=false;
           
        }
        else{
            //Debug.Log("Camera move current entry: " + currentEntry);
            verlagerungScript.resetJumpD(); //-> VerlagerungStatisch
            Vector3 targetPos= new Vector3(camera.transform.position.x, cameraPosY[(currentEntry+1)%5], camera.transform.position.z);
            camera.transform.position= Vector3.Lerp(camera.transform.position, targetPos, 0.02f*Time.deltaTime*framewert);
        }  
    }

    //Timer bis Transition kommt
    void startTimer()
    {
        timerToTransition-= Time.deltaTime;

        if(timerToTransition<0)
        {
            allowMoveCamera=true;
            uiAnimation.SetBool("transition", true); //-> Blendet UI oben aus
            allowTimer=false;
            timerToTransition=1;
        }

    }

    /*void timeFunction()
    {
        timePassed += Time.deltaTime;
        minutes= Mathf.FloorToInt(timePassed / 60);
        seconds= Mathf.FloorToInt(timePassed % 60);
        //timeText.text= "" + minutes +":" + seconds;
        timeText.text= string.Format("{0:00}:{1:00}", minutes, seconds);
    } */

    //Lässt Timer für Transition Starten
    public void allowTimerStart()
    {
        allowTimer=true;
    }

    //Gibt aktuelles Level zurück
    public int getCurrentLevel()
    {
        return currentEntry;
    }

    public void allowLevelAnzeige()
    {
        doLevelAnzeige=true;
    }

    void levelAnzeigeAnimation()
    {
        /*levelKasten[currentEntry].sizeDelta=Vector2.Lerp(levelKasten[currentEntry].sizeDelta, targetSizeKasten, 0.1f);
        levelKasten[currentEntry].anchoredPosition=Vector2.Lerp(levelKasten[currentEntry].anchoredPosition, targetPosKasten[currentEntry], 0.1f);
        if(52-levelKasten[currentEntry].sizeDelta.x<=0.01f)
        {
            levelNumbers[currentEntry].color=darkBlue;
            Debug.Log("Dark gemacht, Current Entry= " +currentEntry);
            levelNumbers[currentEntry+1].color=pink;
            doLevelAnzeige=false;

        }*/

        levelKasten[currentEntry].color=Color.Lerp(levelKasten[currentEntry].color,Color.white,0.1f*Time.deltaTime*framewert);
        levelKasten[currentEntry+1].color=Color.Lerp(levelKasten[currentEntry+1].color,pink,0.1f*Time.deltaTime*framewert);
        if(1-levelKasten[currentEntry].color.r<0.001f)
        {
            levelKasten[currentEntry].color=Color.white;
            levelKasten[currentEntry+1].color=pink;
            doLevelAnzeige=false;
        }


    }

    public void makeNextState()
    {
        transitionStats++;
    }

   
    
}
