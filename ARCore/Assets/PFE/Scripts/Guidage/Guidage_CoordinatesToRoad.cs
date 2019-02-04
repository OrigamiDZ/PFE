using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// this script is attached on a target object which go on the map
public class Guidage_CoordinatesToRoad : MonoBehaviour {
    
    // next scen to load when arrived at the last point
    public string nextScene = "Discovery";

    // list of the coordinates to go to
    public List<Mapbox.Utils.Vector2d> targetsLatLong = new List<Mapbox.Utils.Vector2d>();

    // player object that is used on the map
    public GameObject player;

    // next latitude and longitude to go to
    private double latitude;
    private double longitude;

    // distance between the object and the player object minimum in order to switch to the next coordiantes
    public float distTrigger = 5;

    // switch to next target by removing the first object of the list
    // at the end, load the next scene
    private void NextTarget()
    {
        if (targetsLatLong.Count > 1)
        {
            targetsLatLong.RemoveAt(0);
            latitude = targetsLatLong[0].x;
            longitude = targetsLatLong[0].y;
        }
        else
            SceneManager.LoadScene(nextScene);
    }

    void Awake()
    {
        // need to ask for GPS permision in Awake to have access to it later in the scene, otherwise need to reload the app
        if (AndroidRuntimePermissions.CheckPermission("android.permission.ACCESS_FINE_LOCATION") != AndroidRuntimePermissions.Permission.Granted)
        {
            AndroidRuntimePermissions.RequestPermission("android.permission.ACCESS_FINE_LOCATION");
        }
    }

    // Update the target position on the map
    private void UpdateTargetPosition()
    {
        var map = LocationProviderFactory.Instance.mapManager;
        Mapbox.Utils.Vector2d coord = new Mapbox.Utils.Vector2d(latitude, longitude);
        transform.localPosition = map.GeoToWorldPosition(coord);
    }

    private float CalculateDistanceBetweenPlayerAndtarget()
    {
        return Mathf.Sqrt(Mathf.Pow(player.transform.position.z - transform.localPosition.z, 2) + Mathf.Pow(player.transform.position.x - transform.localPosition.x, 2));
    }

    void Start()
    {
        if (targetsLatLong.Count > 0)
        {
            latitude = targetsLatLong[0].x;
            longitude = targetsLatLong[0].y;
        }
    }

    void LateUpdate()
    {
        UpdateTargetPosition();

        float distance = CalculateDistanceBetweenPlayerAndtarget();

        if (distance < distTrigger && distance != 0) // distance null during the initialisation and we don't want it to switch target
        {
            NextTarget();
        }
    }
}
