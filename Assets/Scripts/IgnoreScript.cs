using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for igonring collider of moving Objects
//---------------------------------------------------------------------------

public class IgnoreScript : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreCollision(object1.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(object2.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(object3.GetComponent<Collider>(), GetComponent<Collider>());


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
