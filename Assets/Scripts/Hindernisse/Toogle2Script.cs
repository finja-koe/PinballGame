using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toogle2Script : MonoBehaviour
{
    public GameObject firstObject;
    public GameObject secondObject;

    public float frontPosZ1;
    public float frontPosZ2;
    public float backPosZ1;
    public float backPosZ2;

    Vector3 posFront1;
    Vector3 posFront2;

    Vector3 posBack1;
    Vector3 posBack2;

    bool move1;
    bool move2;

    private Vector3 velocity1 = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;

    
    public float switchSpeed=0.2f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos1= firstObject.transform.position;
        Vector3 pos2= secondObject.transform.position;

        posFront1= new Vector3(pos1.x,pos1.y,frontPosZ1);
        posFront2= new Vector3(pos2.x,pos2.y,frontPosZ2);

        posBack1= new Vector3(pos1.x,pos1.y,backPosZ1);
        posBack2= new Vector3(pos2.x,pos2.y,backPosZ2);

        secondObject.transform.position=posBack2;


    }

    // Update is called once per frame
    void Update()
    {
        if(move1)
        {
            Debug.Log("Move1");
            firstObject.transform.position=Vector3.SmoothDamp( firstObject.transform.position, posBack1,ref velocity1, switchSpeed);
            secondObject.transform.position=Vector3.SmoothDamp( secondObject.transform.position, posFront2,ref velocity2, switchSpeed);
        }
        else if(move2)
        {
            firstObject.transform.position=Vector3.SmoothDamp( firstObject.transform.position, posFront1,ref velocity1, switchSpeed);
            secondObject.transform.position=Vector3.SmoothDamp( secondObject.transform.position, posBack2,ref velocity2, switchSpeed);
        }
    }

    void OnCollisionEnter(Collision collObject)
    {
        if(collObject.gameObject==firstObject)
        {
            move1=true;
            move2=false;
        }
        else if(collObject.gameObject==secondObject)
        {
            move2=true;
            move1=false;
        }
    }


}
