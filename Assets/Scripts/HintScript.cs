using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{
    bool start;
    float maxTimer=5;
    float timerCnt=0;
    public Animator uiAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {   
            if(timerCnt<maxTimer)     
            {
                timerCnt+= Time.deltaTime;
            }
            else 
            {
                timerCnt=0;
                uiAnimation.SetBool("JumpHint", true);
            }
        }
        else{
            timerCnt=0;
        }
    }

    public void startHint()
    {
        start=true;
    }

    public void leaveHint()
    {
        start=false;
        uiAnimation.SetBool("JumpHint", false);
    }
}
