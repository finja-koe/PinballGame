using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// Calls Functions of other scripts that are needed in the UI Animatior (Idle State)
//------------------------------------------------------------------------------------


public class AnimatorIdleScreen : MonoBehaviour
{

    public SerialScript serialScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadNextScne()
    {
        serialScript.closePort();
        SceneManager.LoadScene(1);
    }

    public void resetGame()
    {
        serialScript.closePort();
        SceneManager.LoadScene(0);
    }
}
