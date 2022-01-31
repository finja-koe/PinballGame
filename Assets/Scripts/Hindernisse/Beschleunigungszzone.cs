using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beschleunigungszzone : MonoBehaviour
{
    public Vector3 forceDir;
    public GameObject ball;
    Rigidbody ballRb;
    // Start is called before the first frame update
    void Start()
    {
        ballRb= ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject==ball)
        {
            ballRb.AddForce(forceDir, ForceMode.Acceleration);

        }
    }
    
    
}
