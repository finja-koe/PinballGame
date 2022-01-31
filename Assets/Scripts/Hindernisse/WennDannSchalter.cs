using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WennDannSchalter : MonoBehaviour
{
    public GameObject ausloeser1;
    public GameObject ausloeser2;
    public AnUndAus anAusScript;

    public Vector3 endPos;
    Vector3 defaultPos;

    void Start()
    {
        defaultPos=transform.position;
    }
    void Update(){
        if(ausloeser1.transform.position.z>4 && ausloeser2.transform.position.z>4)
        {
            Debug.Log("true");
            anAusScript.enabled=false;
            transform.position=Vector3.Lerp(transform.position,endPos,0.01f);
            //transform.position=endPos;
            Debug.Log("pos= " + transform.position);

        }
        else{
            anAusScript.enabled=true;
           // transform.position=Vector3.Lerp(transform.position,defaultPos,0.01f);
        }
    }


}
