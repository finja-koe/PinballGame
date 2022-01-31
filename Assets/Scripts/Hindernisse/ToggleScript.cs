using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleScript : MonoBehaviour
{

    public GameObject otherObject;

    public Vector3 originPosOwn;
    public Vector3 backPosOwn;

    public Vector3 originPosOther;
    public Vector3 backPosOther;

    bool move;

    private Vector3 velocity1 = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;

    
    public float switchSpeed=0.2f;

    // Start is called before the first frame update
    void Start()
    {
 

    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            transform.position=Vector3.SmoothDamp( transform.position, backPosOwn,ref velocity1, switchSpeed);
            //otherObject.transform.position= originPosOther;
            otherObject.transform.position=Vector3.SmoothDamp( otherObject.transform.position, originPosOther,ref velocity2, switchSpeed);
           /* if(transform.position==backPosOwn)
            {
                move=false;
            }*/
        }
    }

    void OnCollisionEnter()
    {
        move=true;
    }
}
