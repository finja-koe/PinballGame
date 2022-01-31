using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AngleTest : MonoBehaviour
{
    public Text jumpDirText;
    float jumpDir;
    float jumpDirMax=5;
    float jumph=5.5f;
    float varTest;
    Vector3 originPos;
    Rigidbody ballRb;
    public GameObject ball;
    // Start is called before the first frame update HelloTest
    void Start()
    {
        originPos  = ball.transform.position;
        ballRb= ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        reset();
        jumpDirText.text=""+jumpDir;
       
        if(Input.GetKey("s") && jumpDir>-jumpDirMax)
        {
            jumpDir-=0.01f;
        }
        if(Input.GetKey("a") && jumpDir<jumpDirMax)
        {
            jumpDir+=0.01f;
        }

        if(Input.GetKeyUp("v"))
        {
            Vector3 jump = new Vector3(jumpDir, jumph, 0f);
            ballRb.AddForce(jump * 2, ForceMode.Impulse);
        }
    

    }

    void reset()
    {
        if(Input.GetKey("r"))
        {
            ball.transform.position= originPos;
            ballRb.Sleep();
        }
    }
}
