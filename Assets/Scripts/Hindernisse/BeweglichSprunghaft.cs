using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeweglichSprunghaft : MonoBehaviour
{

    public Vector3 targetPos1;
    public Vector3 targetPos2;
    public float speed=1f;
    float frameWert=150;

    public bool hasMoveObject=true;
    public GameObject moveObject;


     private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if(hasMoveObject)
        {
            Physics.IgnoreCollision(moveObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(transform.position, targetPos2) <= 0.01f)
        {
                Vector3 remember = targetPos1;
                targetPos1 = targetPos2;
                targetPos2 = remember;
        }
         transform.position = Vector3.SmoothDamp(transform.position, targetPos2, ref velocity, speed);
    }
}
