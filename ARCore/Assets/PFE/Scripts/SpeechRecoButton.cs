using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecoButton : MonoBehaviour {
    public Text statusTextIn;
    private Image image;
	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        string statusText = statusTextIn.text;
        if (statusText == "Status: ERROR_NO_MATCH" || statusText == "Status: EndOfSpeech") {
            image.color = Color.white;
        }
        else if (statusText == "Status: ReadyForSpeech" || statusText == "Status: BeginningOfSpeech") {
            image.color = Color.green;
        }
        else
            image.color = Color.red;
	}
}
