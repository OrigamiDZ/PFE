using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.SceneManagement;

public class SetTargetPlacingOnMap : MonoBehaviour {

    private Mapbox.Utils.Vector2d coordinatesOfTheTarget;

    public void SetCoordinatesOfTheTarget(Mapbox.Utils.Vector2d newCoordinatesOfTheTarget)
    {
        coordinatesOfTheTarget = newCoordinatesOfTheTarget;
    }

    public Mapbox.Utils.Vector2d GetCoordinatesOfTheTarget()
    {
        return coordinatesOfTheTarget;
    }

    void LateUpdate()
    {
        var map = LocationProviderFactory.Instance.mapManager;
        transform.localPosition = map.GeoToWorldPosition(coordinatesOfTheTarget); 
    }
}
