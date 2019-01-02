using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSsandbox_zoneDetection : MonoBehaviour {

    [SerializeField]
    private Transform forumZoneCenter;
    [SerializeField]
    private float forumZoneRadius;

    [SerializeField]
    private Transform gymZoneCenter;
    [SerializeField]
    private float gymZoneRadius;

    [SerializeField]
    private Transform restaurantZoneCenter;
    [SerializeField]
    private float restaurantZoneRadius;

    [SerializeField]
    private Transform foyerZoneCenter;
    [SerializeField]
    private float foyerZoneRadius;

    public Text notificationField;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, forumZoneCenter.position) < forumZoneRadius)
        {
            notificationField.text = "Inside forum zone";
            Debug.Log("forum");
        }
        else if (Vector3.Distance(transform.position, gymZoneCenter.position) < gymZoneRadius)
        {
            notificationField.text = "Inside gym zone";
            Debug.Log("gym");
        }
        else if (Vector3.Distance(transform.position, restaurantZoneCenter.position) < restaurantZoneRadius)
        {
            notificationField.text = "Inside restaurant zone";
            Debug.Log("restaurant");
        }
        else if (Vector3.Distance(transform.position, foyerZoneCenter.position) < foyerZoneRadius)
        {
            notificationField.text = "Inside foyer zone";
            Debug.Log("foyer");
        }
        else
        {
            notificationField.text = " ";
        }
    }
}
