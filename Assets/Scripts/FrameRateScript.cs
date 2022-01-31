using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Display FrameRate
//-----------------------------------------------------------------

public class FrameRateScript : MonoBehaviour
{
    public Text fpsText;
    //public VerlagerungStatisch verlagerungScript;
    float oldJumpD;
    float newJumpD;
    float jumpDiff;
    public float deltaTime=0f;
    int counter;
 
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void Update()
    {
         deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
         float fps = 1.0f / deltaTime;
         fpsText.text = Mathf.Ceil (fps).ToString (); 
         /*if(Input.anyKey)
         {        caluculateJumpDDiff();
        }
                fpsText.text+= " | old= " + oldJumpD + ", new= "+ newJumpD +", Diff= " + jumpDiff; */


       
    }

    void caluculateJumpDDiff()
    {
        oldJumpD=newJumpD;
        //newJumpD=verlagerungScript.getJumpD();
        jumpDiff= oldJumpD-newJumpD;
    }
}
