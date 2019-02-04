using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Class for the UI element event page, setting the right global number of objectives done
public class EventObjectiveGlobal : MonoBehaviour {

    public int totalNbObjectives;

	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = AppController.control.currentObjectiveDoneEvent + " / " + totalNbObjectives.ToString();
    }
}
