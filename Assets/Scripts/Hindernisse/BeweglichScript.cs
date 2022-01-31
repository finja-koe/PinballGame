using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeweglichScript : MonoBehaviour
{

    public Vector3 targetPos1;
    public Vector3 targetPos2;

    public float speed=1f;
    float startTime;
    float journeyLength;
    float frameWert=150;

    // Start is called before the first frame update
    void Start()
    {
        startTime= Time.time; // Zeit zum Startpunkt
        journeyLength= Vector3.Distance(targetPos1,targetPos2);

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Vector3.Distance(transform.position, targetPos2) <= 0.01f)
        {
                Vector3 remember = targetPos1;
                targetPos1 = targetPos2;
                targetPos2 = remember;
                startTime=Time.time;
        }
        float distCovered= (Time.time-startTime) * speed;
        float fractionOfJourney= distCovered/ journeyLength;
       transform.position= Vector3.Lerp(targetPos1,targetPos2,fractionOfJourney*Time.deltaTime*frameWert);
        
    }
}
