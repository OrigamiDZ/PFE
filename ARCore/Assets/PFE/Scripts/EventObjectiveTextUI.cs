using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventObjectiveTextUI : MonoBehaviour {

    int totalObjective = 3;
    int currentObjective;

	// Use this for initialization
	void Start () {
        currentObjective = AppController.control.currentObjectiveDoneEvent;
        GetComponent<Text>().text = currentObjective + " / " + totalObjective;
	}
	
	// Update is called once per frame
	void Update () {
        currentObjective = AppController.control.currentObjectiveDoneEvent;
        GetComponent<Text>().text = currentObjective + " / " + totalObjective;
        if(currentObjective == totalObjective)
        {
            GetComponent<Text>().color = Color.green;
            AppController.control.eventDone = true;
        }
        else
        {
            GetComponent<Text>().color = Color.red;
        }
    }
}
