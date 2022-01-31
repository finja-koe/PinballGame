using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchrumpfendAusloeser : MonoBehaviour
{

    public GameObject shrinkObj1;
    public GameObject shrinkObj2;
    public GameObject impactObj;

    Renderer[] objRend= new Renderer[3];

    int shrinkCounterGlobal;
    int cnt1=0;
    int cnt2=0;

    public int shrinkMax=4;

    public Color[] colors= new Color[5];
    public Vector3[] shrinkSteps= new Vector3[3];
     Vector3[] moveSteps= new Vector3[5];

    Vector3 defaultPos1= new Vector3(0,19.9f,-0.1f);
    Vector3 defaultPos2= new Vector4(0,20.6f,-0.1f);
    Vector3 defaultPos;

    bool shrinking;
    float frameWert=150;
   




    // Start is called before the first frame update
    void Start()
    {
  
        defaultPos= impactObj.transform.position;

        moveSteps[0]= defaultPos;
        moveSteps[1]= new Vector3 (defaultPos.x, defaultPos.y, defaultPos.z+0.1f);
        moveSteps[2]= new Vector3 (defaultPos.x, defaultPos.y, defaultPos.z+0.2f);
        moveSteps[3]= new Vector3 (defaultPos.x, defaultPos.y, defaultPos.z+0.3f);
        moveSteps[4]= new Vector3 (defaultPos.x, defaultPos.y, defaultPos.z+1f);

        objRend[0]= shrinkObj1.GetComponent<Renderer>();
        objRend[1]= shrinkObj2.GetComponent<Renderer>();
        objRend[2]= impactObj.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
       
  
        shrinkObj1.transform.localScale= Vector3.Lerp(shrinkObj1.transform.localScale, shrinkSteps[cnt1],0.1f*Time.deltaTime*frameWert/3);
        shrinkObj2.transform.localScale= Vector3.Lerp(shrinkObj2.transform.localScale, shrinkSteps[cnt2],0.1f*Time.deltaTime*frameWert/3);
        impactObj.transform.position=Vector3.Lerp(impactObj.transform.position, moveSteps[shrinkCounterGlobal], 0.1f*Time.deltaTime*frameWert/3);

        for(int i=0; i<3; i++)
        {
            Color helper= Color.Lerp(objRend[i].material.GetColor("_Color"), colors[shrinkCounterGlobal], 0.1f*Time.deltaTime*frameWert/3);
            objRend[i].material.SetColor("_Color", helper);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject==shrinkObj1)
        {
            if(cnt1<=2)
            {
                cnt1++;
                shrinking=true;
                shrinkCounterGlobal++;
            }
        }
        if(collision.gameObject==shrinkObj2)
        {
            if(cnt2<=2)
            {
                cnt2++;
                shrinking=true;
                shrinkCounterGlobal++;
            }

        }
        
    }

    public void resetObject()
    {
        shrinking=false;
        shrinkCounterGlobal=0;
        cnt1=0;
        cnt2=0;
        shrinkObj1.transform.position= defaultPos1;
        shrinkObj2.transform.position=defaultPos2;

    }

  

    


}
