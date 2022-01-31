using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//check if player is on the mat -> Leave game if not (inGame)
//-------------------------------------------------------------------------------------------


public class LeaveGame : MonoBehaviour
{
    float timesPressed;
    float timesPressedSecond;
    public Animator uiAnimation;
    public Animator lightsAnimation;
    public VerlagerungStatisch verlagerungScript;
    public HeartScript heartScript;
    public SerialScript serialScript;
    float border=300;
    float borderSecond=1000;
    public RectTransform balkenTimer;
    public Text textTimer;

    int counterForLEDs=0;


    bool inGame=true;
    bool cardOpen=false;

    bool inWaitState=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        if( inWaitState)
        {
            if(timesPressed<border)
            {
                if(Input.GetKey("v"))
                {
                    timesPressed+= 100*Time.deltaTime;
                }
                else
                {
                    timesPressed=0;
                }
                
            }

            else
            {
                serialScript.sendState(0);
                uiAnimation.SetTrigger("leaveGameInWait");
                Debug.Log("Tschau!");
                timesPressed=0;
                inGame=true;
                inWaitState=false;
                

            }
        }
        if(inGame)
        {
            if(timesPressed<border)
            {
                if(Input.GetKey("v"))
                {
                    timesPressed+= 100*Time.deltaTime;
                }
                else
                {
                    timesPressed=0;
                }
                
            }
            else
            {
                //serialScript.sendState(8);
                uiAnimation.SetTrigger("resetGame");
                Debug.Log("Tschau!");
                timesPressed=0;
                inGame=false;
                lightsAnimation.SetBool("turnOff", true);
                verlagerungScript.blendOutRichtung();
                verlagerungScript.enabled=false;
                heartScript.enabled=false;
                //SceneManager.LoadScene(0);

            }
        }
        else if(cardOpen)
        {
            if(timesPressedSecond<borderSecond)
            {
                if(Input.GetKey("v"))
                {
                    
                    timesPressedSecond+= 100*Time.deltaTime;
                    
                        balkenTimer.sizeDelta= new Vector2( 570-(545*(timesPressedSecond/1000)), balkenTimer.sizeDelta.y);
                
                    int timer= Mathf.FloorToInt(11-timesPressedSecond/100);
                    textTimer.text= ""+ timer;
                    if(verlagerungScript.enabled=true)
                    {
                        verlagerungScript.blendOutRichtung();
                        verlagerungScript.enabled=false;
                    }
                }
                else
                {
                    serialScript.sendState(1);
                    timesPressedSecond=0;
                    uiAnimation.SetTrigger("dontLeaveGame");
                    cardOpen=false;
                    inGame=true;
                }     
            }
            else{
                uiAnimation.SetTrigger("leaveGame");
                timesPressedSecond=0;
                serialScript.sendState(0);
                cardOpen=false;
                inGame=true;
            }
        }


        
    }

    //Ändert Zeit-Grenze bis man Spiel verlässt
    public void changeToGameMode(bool mode )
    {
        inWaitState=!mode;
        inGame=mode;

    }

    public void resetLeaveGameCard()
    {
        balkenTimer.sizeDelta= new Vector2(560,balkenTimer.sizeDelta.y);
        textTimer.text="10";
        verlagerungScript.enabled=true;
        heartScript.enabled=true;
        verlagerungScript.blendInRichtung();
        verlagerungScript.jumpAfterLeaveBack();
        }

    public void setInGameTrue()
    {
        inGame=true;
    }

    public void setCardOpenTrue()
    {
        cardOpen=true;
    }

    public void turnLightsOn()
    {
        lightsAnimation.SetBool("turnOff",false);    
    }

   

    

}
