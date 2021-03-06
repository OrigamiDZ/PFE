﻿using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.SceneManagement;

public class RoadToENSIIE : MonoBehaviour {

    public string nextScene = "Discovery";

    public List<Mapbox.Utils.Vector2d> targetsLatLong = new List<Mapbox.Utils.Vector2d>();

    private GameObject player;

    private double latitude;
    private double longitude;
    public int distTrigger = 5;

    private void NextTarget() {
        if (targetsLatLong.Count > 1) {
            targetsLatLong.RemoveAt(0);
            latitude = targetsLatLong[0].x;
            longitude = targetsLatLong[0].y;

        }
        else
            SceneManager.LoadScene(nextScene);
    }
    //il faut mettre la demande de permission du GPS dans le Awake pour y avoir acces ensuite dans la scene, sinon il faudrait
    //redemarrer l application
    void Awake() {
        if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") != AndroidRuntimePermissions.Permission.Granted) {
            AndroidRuntimePermissions.RequestPermission("android.permission.ACCESS_FINE_LOCATION");
        }
    }

    void Start() {
        if (targetsLatLong.Count == 0) {
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.624522, 2.439917));//first step
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.625093, 2.437291));
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.625075, 2.433790));
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.625863, 2.433811));
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.625986, 2.432257));
            targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.626704, 2.432356));//ensiie point
        }

        player = GameObject.FindGameObjectWithTag("Player");

        latitude = targetsLatLong[0].x;
        longitude = targetsLatLong[0].y;
    }

    void LateUpdate() {
        var map = LocationProviderFactory.Instance.mapManager;
        Mapbox.Utils.Vector2d coord = new Mapbox.Utils.Vector2d(latitude, longitude);
        transform.localPosition = map.GeoToWorldPosition(coord); 

        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.z - transform.localPosition.z, 2) + Mathf.Pow(player.transform.position.x - transform.localPosition.x, 2));
        //la distance est nulle pdt l initialisation et l on ne veut pas que ca change de target
        if (distance < distTrigger && distance != 0) {
            NextTarget();
        }
    }
}
