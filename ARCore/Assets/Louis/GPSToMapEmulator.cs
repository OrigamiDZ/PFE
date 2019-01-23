using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSToMapEmulator : MonoBehaviour {
    private double refLat = 48.622811;
    private double refLong = 2.440819;
    private GameObject player;
    public double coefLongitude;
    public double coefLatitude;

    public double inputLatitude = 48.622811;
    public double inputLongitude = 2.440819;
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");


        coefLongitude = 4635 / (2.446167 - refLong);
        coefLatitude = 4635 / (48.626362 - refLat);
    }
	
	// Update is called once per frame
	void Update () {
        player.transform.position = new Vector3(
            player.transform.position.x,
            (float)((inputLatitude - refLat) * coefLatitude + 5),
            player.transform.position.z);

        player.transform.position = new Vector3(
            (float)((inputLongitude - refLong) * coefLongitude + 777),
            player.transform.position.y,
            player.transform.position.z);
    }
}
