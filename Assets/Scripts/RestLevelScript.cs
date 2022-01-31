using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Resets Levels after try again
//-------------------------------------------------------------------


public class RestLevelScript : MonoBehaviour
{
    public Schrumpfend[] schrumpfendScripts;
    public Wachsend[] wachsendScripts;
    public Schalter[] schalterScripts;
    public SchrumpfendAusloeser schrumpfendAusloeser;
    public GameObject[] drehLevel1;
    bool[] drehLevelOrigin;
    public HintScript hintScript;



    // Start is called before the first frame update
    void Start()
    {    
        drehLevelOrigin= new bool[drehLevel1.Length];   
        for(int a=0; a<drehLevel1.Length; a++)
        {
            drehLevelOrigin[a]=drehLevel1[a].GetComponent<HingeJoint>().useSpring;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        int leng= schrumpfendScripts.Length; 
    }

    public void resetLevels()
    {
        for(int i=0; i< schrumpfendScripts.Length; i++)
        {
            schrumpfendScripts[i].resetObject();
        }
        for( int j=0; j< wachsendScripts.Length; j++)
        {
            wachsendScripts[j].resetObject();
        }
        for(int k=0; k<schalterScripts.Length; k++)
        {
            schalterScripts[k].resetObject();
        }

        for(int l=0; l<drehLevel1.Length; l++)
        {
            drehLevel1[l].GetComponent<HingeJoint>().useSpring=true;
            drehLevel1[l].transform.rotation= Quaternion.Euler(0, 0, 0);

        }

        schrumpfendAusloeser.resetObject();
        hintScript.leaveHint();
    }

    public void releaseSprings()
    {
        for(int l=0; l<drehLevel1.Length; l++)
        {
            drehLevel1[l].transform.rotation= Quaternion.Euler(0, 0, 0);
            drehLevel1[l].GetComponent<HingeJoint>().useSpring=drehLevelOrigin[l];
        }
    }
}
