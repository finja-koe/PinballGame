using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Check if the ball is in the shell (Onboarding) 
//-------------------------------------------------------------


public class OnboardingCheckWin : MonoBehaviour
{
    public GameObject canvas;
    OnboardingScript onboardingScript;
    public SerialScript serialScript;

    // Start is called before the first frame update
    void Start()
    {
        onboardingScript=canvas.GetComponent<OnboardingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collidingObject)
    {
        serialScript.sendState(5);
        onboardingScript.setInShell();

    }
}
