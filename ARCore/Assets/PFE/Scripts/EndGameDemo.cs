using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameDemo : MonoBehaviour {

	public void ResetDemoAndGoToDiscovery()
    {
        AppController.control.tutorialDone = false;
        AppController.control.eventDone = false;
        AppController.control.currentObjectiveDoneEvent = 0;
        SceneManager.LoadScene("Discovery");
    }

}
