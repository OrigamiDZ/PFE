using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulleDisabling : MonoBehaviour {

    public Text textResult;
    private Text oldText;
    private float lastChangeTime;

    void Start () {
        oldText = textResult;
        lastChangeTime = Time.time;
        //gameObject.SetActive(false);
    }
    void Update () {
		if (oldText != textResult) {
            lastChangeTime = Time.time;
            //gameObject.SetActive(true);

        }
        oldText = textResult;
        if (Time.time - lastChangeTime > 10) {
            //gameObject.SetActive(false);

        }
    }
}
