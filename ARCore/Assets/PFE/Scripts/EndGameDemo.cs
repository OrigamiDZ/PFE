using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Class used for the end of the presentation demo
public class EndGameDemo : MonoBehaviour {

	public void ResetAndGoToDiscovery()
    {
        //Demo presentation PFE -> resets both the tutorial and the event to their "not done yet" state + switches to discovery mode
        //AppController.control.tutorialDone = false;
        //AppController.control.eventDone = false;
        //AppController.control.currentObjectiveDoneEvent = 0;
        //SceneManager.LoadScene("Discovery");

        //Normal version -> switches to discovery mode
        SceneManager.LoadScene("Discovery");
    }

}
