using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.SceneManagement;

public class PlaceObject : MonoBehaviour {

    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        var map = LocationProviderFactory.Instance.mapManager;
        Mapbox.Utils.Vector2d gymnaseCoord = new Mapbox.Utils.Vector2d(48.625843, 2.44165);
        transform.localPosition = map.GeoToWorldPosition(gymnaseCoord); //48.625843, 2.44165

        float distance = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.localPosition.x, 2) + Mathf.Pow(player.transform.position.x - transform.localPosition.x, 2));

        if (distance < 10)
        {
            SceneManager.LoadScene("MiniGame_hidenseek");
        }
    }
    
}
