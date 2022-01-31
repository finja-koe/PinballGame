using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


//Script for all UI Elements -> Enabel/Disable UI, Starts/Stops Animations, communicate GameStats
//-----------------------------------------------------------------------------------------

public class HeartScript : MonoBehaviour
{
    public Image[] lives =new Image[5];
    public string[] textArrayWelldone= new string[3];
    public TextMeshProUGUI wellDoneText;   
    public TextMeshProUGUI timeLeftText;
  
    public GameObject wellDoneUI;
    public GameObject tryAgainUI;
    public GameObject gameOverUI;
    public GameObject winUI;
    public Animator uiAnimation;
    public Animator lightsAnimation;

    public GameObject area;
    public GameObject ball;
    public SerialScript serialScript;
    public HintScript hintScript;
    VerlagerungStatisch steuerung;
    TeleportScript teleportScript;
    LeaveGame leaveScript;
    RestLevelScript resetScript;

    public Sprite herzVoll;
    public Sprite herzLeer;

    int heartCounter=5;
    int currentLevel=1;

    float timePassed;
    float maxTime= 90; //90
    float timeLeft;
    public TextMeshProUGUI timeText;
    //public Text levelText;
    //public Text allStarsCollectedText;
    float minutes;
    float seconds;

    bool isWellDone=false;
    bool isTryAgain=false;
    bool isGameOver=false;
    bool isWin=false;
    bool isTime=true;

    int[] stars= new int[5];
    public Image[] starsImg= new Image[3];
    public Sprite starVoll;
    public Sprite starLeer;

    float saveTimer=1;
    float saveTimerPassed=0;
    bool saveTimerOk=false;
    string timerText;

    public Image[] starsWinImages= new Image[5];
    public Image[] starsGameOverImages= new Image[5];
    public Sprite[] starsUiImages= new Sprite[4];
    public Image[] gameOverUIKasten= new Image[5];
    public Text[] gameOverUILevelNr= new Text[5];
    public Color white;
    public Color darkBlue;
    public Color pink;
    //Vector2 outlineSize;
    float framewert=150;


    

    // Start is called before the first frame update
    void Start()
    {
        timeLeft=maxTime;
        isTime=false;
        steuerung=area.GetComponent<VerlagerungStatisch>();
        teleportScript=ball.GetComponent<TeleportScript>();
        leaveScript=ball.GetComponent<LeaveGame>();
        resetScript= ball.GetComponent<RestLevelScript>();
        //GameObject outlineObj= gameOverUIKasten[0].gameObject;
        //outlineSize= outlineObj.GetComponent<RectTransform>().sizeDelta - new Vector2(4,6);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("SaveTimerOK: "+ saveTimerOk + " / timePassed: " + saveTimerPassed);
        if(!isWellDone && !isTryAgain && !isGameOver && !isWin && isTime)
        { 
            timeFunction();
            checkLessTimeLeft();
        }
        if(!isWellDone && !isTryAgain && !isGameOver && !isWin)
        {       
             checkTimeOver();
        }


        if(isWellDone)
        {
            wellDoneFunc();
        }
        if(isTryAgain)
        {
            tryAgainFunc();
        }
        if(isGameOver)
        {
            gameOverFunc();
        }
        if(isWin)
        {
            youWinFunc();
        }
    }

    //Berechnet die Zeit und passt Text an
     void timeFunction()
    {
        timePassed += Time.deltaTime;
        timeLeft= maxTime-timePassed;
        minutes= Mathf.FloorToInt(timeLeft / 60);
        seconds= Mathf.FloorToInt(timeLeft % 60);
        //timeText.text= "" + minutes +":" + seconds;
        timerText=string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text= timerText;
        //levelText.text= "Level " + currentLevel;
    }

    //Setzt Time auf Anfang
    public void resetTime()
    {
        timePassed=0;
        timeLeft= maxTime-timePassed;
        minutes= Mathf.FloorToInt(timeLeft / 60);
        seconds= Mathf.FloorToInt(timeLeft % 60);
        //timeText.text= "" + minutes +":" + seconds;
        timerText=string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.text= timerText;
        uiAnimation.SetBool("transition", false);

    }

    //Checkt ob Zeit ball vorbei und passt ggf UI an
    void checkLessTimeLeft()
    {
        if(timeLeft<10 && timeLeft>0)
        {
            uiAnimation.SetBool("LessTimeLeft",true); //-> UI Time wackelt
        }
        else
        {
            uiAnimation.SetBool("LessTimeLeft",false);
        }
    }

    //checkt ob Zeit vorbei
    void checkTimeOver()
    {
        if(timeLeft<1)
        {
            
            uiAnimation.SetBool("LessTimeLeft",false);
            lightsAnimation.SetBool("turnOff",true);
            Debug.Log("HierLights");
            steuerung.setJumpfZero();


            //resetTime();
            if(heartCounter>1)
            {
                isTryAgain=true;
                saveTimerOk=false;
                saveTimerPassed=0;
                resetScript.resetLevels(); // ->RestLevel
                
                

            }
            else
            {
                isGameOver=true;
                saveTimerOk=false;
                saveTimerPassed=0;
            }
            
            if(timeLeft<=1 && timeLeft >=-1)
                {
                    //serialScript.sendState(6);
                    timeLeft=-2;
                    hintScript.leaveHint();
                }

        }
    }

    //Game Over UI anpassen
    void gameOverFunc()
    {
        saveTimerFunc();
        //uiAnimation.SetBool("transition", true);
        uiAnimation.SetBool("gameOver", true);
        leaveScript.changeToGameMode(false); //Spiel wird schneller verlassen wenn man von matte runter

        //GameObject outlineKasten=  gameOverUIKasten[teleportScript.getCurrentLevel()+1].gameObject;
        //outlineKasten.GetComponent<Outline>().enabled=true;
        //outlineKasten.GetComponent<RectTransform>().sizeDelta= outlineSize;

        for(int i=0; i<5; i++)
        {
            starsGameOverImages[i].sprite=starsUiImages[stars[i]];
            if(stars[i]>0)
            {
                gameOverUILevelNr[i].color=white;
                gameOverUIKasten[i].color=darkBlue;
            }

        }
        gameOverUIKasten[teleportScript.getCurrentLevel()+1].color=pink;
        steuerung.blendOutRichtung();
        steuerung.enabled=false;
        timeLeft=0;
        timeText.text= "00:00";
        if(Input.GetKeyUp("v") && saveTimerOk)
        {
            serialScript.sendState(4);
            leaveScript.changeToGameMode(true); // Spiel wird wieder nach normaler Zeit verlassen
            
            uiAnimation.SetBool("gameOver", false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown("v"))
        {
            serialScript.sendState(3);
        }
    }


    void tryAgainFunc()
    {
        isTime=false;
        saveTimerFunc();
        leaveScript.changeToGameMode(false); 
        //Debug.Log("TryAgain");
        uiAnimation.SetBool("noTimeLeft", true);
        steuerung.resetDirTimer();
        steuerung.blendOutRichtung();
        steuerung.enabled=false;
        timeLeft=0;
        timeText.text= "00:00";


        if(Input.GetKeyUp("v") && saveTimerOk)
        {
            serialScript.sendState(4);
            resetScript.resetLevels();
            resetScript.releaseSprings();
            
            leaveScript.changeToGameMode(true);
            uiAnimation.SetBool("noTimeLeft", false);
            lightsAnimation.SetBool("turnOff",false);
            heartCounter--;
            lives[heartCounter].sprite=herzLeer;
            steuerung.blendOutRichtung();
            steuerung.enabled=true;
            teleportScript.retryBallPos();
            resetTime();
            isTryAgain=false;
        }
        if(Input.GetKeyDown("v"))
        {
            serialScript.sendState(3);
        }
 
    }

    void youWinFunc()
    {
        saveTimerFunc();
        int starCollected= calculateStars(timeLeft, maxTime);
        stars[4]=starCollected;
        for(int i=0; i<5; i++)
        {
            starsWinImages[i].sprite=starsUiImages[stars[i]];
        }
        leaveScript.changeToGameMode(false);
        uiAnimation.SetBool("LessTimeLeft", false);
        uiAnimation.SetBool("youWin",true);
        steuerung.blendOutRichtung();
        steuerung.enabled=false;
        int allStars=0;
        for(int i=0; i<5;i++)
        {
            allStars+=stars[i];
        }
        //allStarsCollectedText.text= allStars+ "/15            collected";
        if(Input.GetKeyUp("v") && saveTimerOk)
        {
            serialScript.sendState(4);
            leaveScript.changeToGameMode(true);
            uiAnimation.SetBool("youWin",false);
            //lightsAnimation.SetBool("turnOff",false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(Input.GetKeyDown("v"))
        {
            serialScript.sendState(3);
        }
    }

    void wellDoneFunc()
    {
        saveTimerFunc();
        steuerung.resetDirTimer();
        timeLeftText.text=timerText;
        int starCollected= calculateStars(timeLeft, maxTime);
        wellDoneText.text=textArrayWelldone[starCollected-1];
        uiAnimation.SetBool("LessTimeLeft", false);

        //Star imgs anpassen
        for(int i=0; i<starCollected; i++)
        {
            starsImg[i].sprite=starVoll;
        }
        stars[teleportScript.getCurrentLevel()]=starCollected; //Speichert stars anzahl
        uiAnimation.SetBool("wellDone", true);
        leaveScript.changeToGameMode(false);
        steuerung.blendOutRichtung();
        steuerung.enabled=false;
        isTime=false;
        if(Input.GetKeyUp("v") && saveTimerOk)
        {
            serialScript.sendState(4);
            leaveScript.changeToGameMode(true);
            uiAnimation.SetBool("wellDone", false);
            lightsAnimation.SetBool("turnOff",false);
            wellDoneUI.SetActive(false);
            teleportScript.allowTimerStart();
            steuerung.enabled=true;
            isWellDone=false;
            Debug.Log("Stars= " + stars[0] + stars [1] + stars [2] + stars[3] + stars[4]);
        }
        if(Input.GetKeyDown("v"))
        {
            serialScript.sendState(3);
        }

    }

    public void doWellDoneFunc( int nxtLevel)
    {
        steuerung.setJumpfZero();
        isWellDone=true;
        lightsAnimation.SetBool("turnOff",true);
        currentLevel=nxtLevel;
        saveTimerOk=false;
        saveTimerPassed=0;
    }

    public void doWinFunc()
    {
        steuerung.setJumpfZero();
        lightsAnimation.SetBool("turnOff",true);
        isWin=true;
        saveTimerOk=false;
        saveTimerPassed=0;
    }

    public void doTime()
    {
        isTime=true;
        
    }

    //Gibt aus wie viele Stars man kriegt
    int calculateStars( float timeLeftCurrent, float maxTime)
    {
        float drittleOfMax= maxTime/3;
        if(timeLeftCurrent<=drittleOfMax && timeLeftCurrent >= 0)
        {
            return 1;
        }
        else if(timeLeftCurrent <=drittleOfMax*2)
        {
            return 2;
        }
        else if(timeLeftCurrent <= drittleOfMax*3)
        {
            return 3;
        }
        else
        {
            return 0;
        }

    }

    public void resetStars()
    {
        for(int i=0; i<3; i++)
        {
            starsImg[i].sprite=starLeer;
        }
    }

    public void resetScene()
    {
        serialScript.closePort();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    void saveTimerFunc()
    {
        saveTimerPassed+= Time.deltaTime;
        if(saveTimerPassed>1)
        {
            saveTimerPassed=0;
            saveTimerOk=true;
        }

    }

    public void turnLightsOfWin()
    {
        lightsAnimation.SetTrigger("lightsOffWin");
        lightsAnimation.SetBool("turnOff",true);
    }

    public bool returnIsTimeOver()
    {
        if(isGameOver || isTryAgain)
        {
            return true;
        }
        else{
            return false;
        }
    }

}
