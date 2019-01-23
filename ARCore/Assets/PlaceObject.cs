using UnityEngine;
using Mapbox.Unity.Location;

public class PlaceObject : MonoBehaviour {


    void Start()
    {
    }

    void LateUpdate()
    {
        var map = LocationProviderFactory.Instance.mapManager;
        Mapbox.Utils.Vector2d gymnaseCoord = new Mapbox.Utils.Vector2d(48.625843, 2.44165);
        transform.localPosition = map.GeoToWorldPosition(gymnaseCoord); //48.625843, 2.44165
    }
    
}
