using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Calls Functions of other scripts that are needed in the UI Animatior (InGame)
//------------------------------------------------------------------------------------


public class AnimatorCanvas : MonoBehaviour
{
    public HeartScript heartScript;
    public TeleportScript teleportScript;
    public LeaveGame leaveGameScript;
    public ParticleSystem konfettiR;
    public ParticleSystem konfettiL;
    public GameObject konfettiRObj;
    public GameObject konfettiLObj;
    public SerialScript serialScript;
    public HintScript hintScript;

    Animator uiAnimator;
    public CountdownScript countdownScript;


    // Start is called before the first frame update
    void Start()
    {
        konfettiL.Stop();
        konfettiR.Stop(); 

        uiAnimator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetStars()
    {
        heartScript.resetStars();
    }

    public void resetScene()
    {
        heartScript.resetScene();
    }

    public void startKonfetti()
    {
        konfettiLObj.SetActive(true);
        konfettiRObj.SetActive(true);
        konfettiL.Play();
        konfettiR.Play();
    }

    public void stopKonfetti()
    {
        konfettiLObj.SetActive(false);
        konfettiRObj.SetActive(false);
        konfettiL.Stop();
        konfettiR.Stop();
    }

    public void resetGame()
    {
        serialScript.closePort();
        SceneManager.LoadScene(0);

    }

    public void startLevelAnzeige()
    {
        teleportScript.allowLevelAnzeige();
    }

    public void turnLightOffWin()
    {
        heartScript.turnLightsOfWin();
    }

    public void startCountdown()
    {
        countdownScript.startTimerFunc();
    }

    public void setInGameTrue()
    {
        leaveGameScript.setInGameTrue();
    }

    public void setCardOpenTrue()
    {
        leaveGameScript.setCardOpenTrue();
        serialScript.sendState(8);
    }

    public void resetLeaveGameCard()
    {
        leaveGameScript.resetLeaveGameCard();
    }

    public void turnLightsOn()
    {
        leaveGameScript.turnLightsOn();
    }

    public void sendState( int x)
    {
        serialScript.sendState(x);
    }

    public void resetJumpHint()
    {
        hintScript.leaveHint();
        
    }

    public void startHint()
    {
        hintScript.startHint();
    }

    




   



}
