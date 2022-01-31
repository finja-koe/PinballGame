using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//check if player is on the mat -> Leave game if not (Idle)
//-------------------------------------------------------------------------------------------

public class LeaveIdle : MonoBehaviour
{

    float timesPressed;
    public Animator uiAnimation;
    float border=300;

    public SerialScript serialScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timesPressed<border)
        {
            if(Input.GetKey("v"))
            {
                timesPressed+= 100*Time.deltaTime;
            }
            else{
                timesPressed=0;
            }
        }
        else
        {
            uiAnimation.SetTrigger("resetGame");
            timesPressed=0;
            serialScript.sendState(0);
        }
        
    }
}
