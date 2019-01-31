using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.UI;

public class RoadToGymnase : MonoBehaviour {

    public string nextScene = "Discovery";

    private List<Mapbox.Utils.Vector2d> targetsLatLong = new List<Mapbox.Utils.Vector2d>();

    public GameObject player;

    public Text debug;

    private double latitude;
    private double longitude;
    public float distTrigger;

    private void NextTarget()
    {
        if (targetsLatLong.Count > 1)
        {
            targetsLatLong.RemoveAt(0);
            latitude = targetsLatLong[0].x;
            longitude = targetsLatLong[0].y;
        }
        /*else
            SceneManager.LoadScene(nextScene);*/
    }



    void Start()
    {
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62510372656877, 2.443163504975189));//first step
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62537888811181, 2.443486173793417));
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62545252924033, 2.443165463900015));
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62543749434482, 2.442110213498267));
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62583119688065, 2.442093340171539));
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62581378817185, 2.441719775759992));
        targetsLatLong.Add(new Mapbox.Utils.Vector2d(48.62574804352428, 2.441374468422879));

        latitude = targetsLatLong[0].x;
        longitude = targetsLatLong[0].y;
    }

    void LateUpdate()
    {
        var map = LocationProviderFactory.Instance.mapManager;
        Mapbox.Utils.Vector2d coord = new Mapbox.Utils.Vector2d(latitude, longitude);
        transform.localPosition = map.GeoToWorldPosition(coord);

        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.z - transform.localPosition.z, 2) + Mathf.Pow(player.transform.position.x - transform.localPosition.x, 2));

        if (distance < distTrigger && distance != 0)
        {//la distance est nulle pdt l initialisation et l on ne veut pas que ca change
            //de target
            NextTarget();
        }
        debug.text = "Target n°" + (8 - targetsLatLong.Count) + " : " + distance;

    }
}
