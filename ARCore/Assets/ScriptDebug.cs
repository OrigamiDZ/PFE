using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;

public class ScriptDebug : MonoBehaviour {

    public Text myText;
    public DeviceLocationProvider locationProvider;
    public GetDirection myDirection;

	// Use this for initialization
	void Start () {
        //Input.location.Start();
    }
	
	// Update is called once per frame
	void Update () {
        myText.text =
            "IsLocationServiceEnabled : " + locationProvider.CurrentLocation.IsLocationServiceEnabled +
            "\n" +
            "UserHeading : " + locationProvider.CurrentLocation.UserHeading +
            "\n" +
            "DeviceOrientation : " + locationProvider.CurrentLocation.DeviceOrientation +
            "\n" +
            "DirectionToGo : " + myDirection.GetAngle();
	}
}
