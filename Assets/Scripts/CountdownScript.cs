using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Countdown befor Level 1
//-----------------------------------------------------


public class CountdownScript : MonoBehaviour
{
    public Text countdownText;

    float passedTime;
    float maxTime=3;
    float roundTime;

    public GameObject area;
    public GameObject ball;
    Rigidbody ballRb;
    public GameObject UIObject;
    public GameObject countdown;

    public TeleportScript teleportScript;
    HeartScript uiScript;
    VerlagerungStatisch steuerungScript;
    public Animator lightsAnimation;

    bool startCountdown=false;


   


    // Start is called before the first frame update
    void Start()
    {

        passedTime=maxTime;

        steuerungScript=area.GetComponent<VerlagerungStatisch>();
        uiScript=UIObject.GetComponent<HeartScript>();

        steuerungScript.enabled=false;
        uiScript.enabled=false;
        ballRb= ball.GetComponent<Rigidbody>();
        ballRb.Sleep();
        



    }

    // Update is called once per frame
    void Update()
    {
        if(startCountdown)
        {
        passedTime-= Time.deltaTime;
        countdownText.color= new Color (255,255,255,255);

        if(passedTime>0.4f)
        {
            roundTime=Mathf.Round(passedTime+0.4f); 
            countdownText.text=""+roundTime;
        }
        else{
            uiScript.enabled=true;
            ballRb.WakeUp();
            countdown.SetActive(false);
            teleportScript.retryBallPos();
            lightsAnimation.SetTrigger("startGame");
        }
        }

    }

    public void startTimerFunc()
    {
        startCountdown=true;
    }
}
