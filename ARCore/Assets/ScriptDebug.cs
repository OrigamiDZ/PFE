using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;

public class ScriptDebug : MonoBehaviour {

    public Text myText;
    public DeviceLocationProvider locationProvider;
    public GetDirection myDirection;

    public GameObject DebugText1;
    public GameObject DebugText2;

    public Text buttonText;

    private int tour;


    // Use this for initialization
    void Start () {
        //Input.location.Start();
        tour = 0;
    }
    public void SwitchDebug()
    {
        switch (tour % 3)
        {
            case 0:
                DebugText1.SetActive(true);
                DebugText2.SetActive(false);
                break;
            case 1:
                DebugText1.SetActive(false);
                DebugText2.SetActive(true);
                break;
            case 2:
                DebugText1.SetActive(false);
                DebugText2.SetActive(false);
                break;
            default:
                break;
        }
        tour++;
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
