using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinScript : MonoBehaviour
{

    public GameObject ball;

    Vector3 inDirection;
    Vector3 outDirection;

    Rigidbody ballRb;
    // Start is called before the first frame update
    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==ball)
        {
            Debug.Log("in:" + ballRb.velocity);
            inDirection= ballRb.velocity;
            outDirection=  Vector3.Reflect(inDirection, Vector3.right);
            ballRb.velocity= outDirection*2;
            //ballRb.AddForce(outDirection*1, ForceMode.Impulse);

            Debug.Log("in: " + inDirection + " Out: " + outDirection);
        }
    }

    /*void OnCollisionExit()
    {
        ballRb.AddForce(outDirection*2, ForceMode.Acceleration);
        Debug.Log("out:" + ballRb.velocity);


    }*/
}
