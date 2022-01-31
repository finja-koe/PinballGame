using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipOnboarding : MonoBehaviour
{
    bool inLuft;
    bool inLuftCard;
    bool cardOpen;

    float timerCnt;

    int saveCnt;

    public Animator uiAnimation;
    public Animator lightsAnimation;
    public OnboardingLeaveScript leaveScript;
    public SerialScript serialScript;

    public RectTransform balkenTimer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyUp("v"))
        {
            uiAnimation.SetTrigger("skipGame");
            serialScript.sendState(4);
        }
        if(Input.GetKeyDown("v"))
        {
             serialScript.sendState(3);
             cardOpen=false;
        }
        else{
           
            if(timerCnt>=5)
            {
                uiAnimation.SetTrigger("dontSkipGame");
            }
        }
        if(cardOpen)
        {
            timerCnt+= Time.deltaTime;
            if(timerCnt<5)
            {
                balkenTimer.sizeDelta= new Vector2( 570-(545*(timerCnt/5)), balkenTimer.sizeDelta.y);
            }
        }


        /*if(!cardOpen)
        {
            if(Input.GetKey("v"))
            {
                saveCnt++;
                if(saveCnt>2)
                {
                    inLuft=true;
                    
                }
            }
            else if(Input.GetKeyUp("v") && inLuft)
            {
                uiAnimation.SetTrigger("startSkipGame");
                lightsAnimation.SetBool("TurnOff", true);
                cardOpen=true;
                leaveScript.enabled=false;
                saveCnt=0;
            }
            else{
                saveCnt=0;
            }
        }

        if(cardOpen)
        {
            timerCnt+= Time.deltaTime;
            if(timerCnt>=5)
            {
                uiAnimation.SetTrigger("skipGame");
            }

            if(Input.GetKey("v"))
            {
                inLuftCard=true;
            }
            if(Input.GetKeyDown("v") && inLuftCard)
            {
                uiAnimation.SetTrigger("dontSkipGame");
                leaveScript.enabled=true;
                inLuftCard=false;
                saveCnt=0;
                inLuft=false;
                timerCnt=0;
                cardOpen=false;
                //uiAnimation.ResetTrigger("dontSkipGame");
                uiAnimation.ResetTrigger("startSkipGame");

            }
        } */
    }

    public void setCardOpen()
    {
        cardOpen=true;
    }

}
