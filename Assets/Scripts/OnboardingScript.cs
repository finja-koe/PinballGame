using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script for Animation Onboarding, Comunitication with mat
//-----------------------------------------------------------

public class OnboardingScript : MonoBehaviour
{
    public GameObject ball;
    public GameObject area;
    public OnboardingLeaveScript leaveOnScript;
    public OnboardingLearning learning;
    public SerialScript serialScript;
    public SkipOnboarding skipScript;
    SteuerungOnboarding steuerung;
    //public LeaveGame leaveScript;
    Rigidbody ballRb;
    Animator uiAnimation;
    public Animator lightAnimation;
    bool inShell=false;


    void Start(){
        ballRb=ball.GetComponent<Rigidbody>();
        steuerung=area.GetComponent<SteuerungOnboarding>();
        uiAnimation=this.GetComponent<Animator>();
        ballRb.Sleep();
        
    }

    void Update(){
        if(inShell)
        {
            if(Input.GetKeyUp("v"))
            {
                serialScript.sendState(4);
                uiAnimation.SetBool("fadeOut", true);

            }
            if(Input.GetKeyDown("v"))
            {
                serialScript.sendState(3);
            }
        }
        
    }

    public void awakeBall()
    {
        ballRb.WakeUp();
        //steuerung.enabled=true;
    }

    public void jumpNow()
    {
        uiAnimation.SetTrigger("ShiftTimeOver");
    }

    public void resetShiftTimeOver()
    {
        uiAnimation.ResetTrigger("ShiftTimeOver");
    }

    public void jumpedFunc()
    {
        uiAnimation.SetTrigger("Jumped");
    }

    public void setOnArea(bool state)
    {
        uiAnimation.SetBool("onArea", state);
    }
 

    public void setInShell()
    {
        uiAnimation.SetTrigger("inShell");
        inShell=true;
        leaveOnScript.changeToGameMode(false);
    }

    public void loadNextScne()
    {
        
        leaveOnScript.changeToGameMode(true);
        SceneManager.LoadScene(2);
    }

    public void resetGame()
    {
        serialScript.closePort();
        SceneManager.LoadScene(0);
    }

    public void turnLightOn()
    {
        serialScript.closePort();
        lightAnimation.SetTrigger("TurnOn");
    }

     public void turnLightOff()
    {
        lightAnimation.SetTrigger("TurnOff");
    }

    public void setCardOpenTrue(){
        leaveOnScript.setCardOpenTrue();
        serialScript.sendState(8);
    }

    public void resetLeaveGameCard()
    {
        leaveOnScript.resetLeaveGameCard();
    }

    public void disableLearning()
    {
        learning.enabled=false;
    }

    public void disableSteuerung()
    {
        steuerung.setInActive();
    }

    public void enableSteuerung()
    {
        //learning.enabled=false;
        steuerung.setActive();
    }

    public void setBallOnPlatform()
    {
        steuerung.setBallonPlatform();
        
    }
    public void setJumpAllowed()
    {
        steuerung.setJumpAllowed();
    }

    public void setStateSerial( int x)
    {
        serialScript.sendState(x);
    }

    public void setSkipCardOpen()
    {
        skipScript.setCardOpen();
    }

    public void sendState(int state)
    {
        serialScript.sendState(state);
    }

}
