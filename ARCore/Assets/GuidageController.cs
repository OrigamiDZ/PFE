using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.UI;


public class GuidageController : MonoBehaviour {

    private float cameraOffSet;
    public Camera cameraPlayer;
    private Vector3 lastPosition;

    public Text debugText;

    private Dictionary<float, Vector3> dictionary;

    public DeviceLocationProvider deviceLocationProvider;

    public GameObject guidage;
    public GameObject initialisationCanvas;

    public float GetCameraOffset()
    {
        return cameraOffSet;
    }

    void AddValuesToDictionary()
    {
        if (deviceLocationProvider.CurrentLocation.IsLocationServiceEnabled)
        {
            if (deviceLocationProvider.CurrentLocation.IsUserHeadingUpdated)
            {
                Vector3 directionVector = cameraPlayer.transform.position - lastPosition;
                dictionary.Add(deviceLocationProvider.CurrentLocation.UserHeading, Vector3.Normalize(directionVector));
                lastPosition = cameraPlayer.transform.position;
            }
        }
    }

    void TryToFindInterestingValue()
    {
        string debug = "";
        Dictionary<float, float> scores = new Dictionary<float, float>(); //offset, score
        foreach (KeyValuePair<float, Vector3> item in dictionary)
        {
            float score = 0;
            foreach (KeyValuePair<float, Vector3> itembis in dictionary)
            {
                if (item.Key != itembis.Key)
                {
                    float distanceBetweenItemAndItembis = Mathf.Abs(item.Key - itembis.Key) + Mathf.Sqrt(
                        Mathf.Pow(item.Value.x - itembis.Value.x, 2) +
                        Mathf.Pow(item.Value.y - itembis.Value.y, 2) +
                        Mathf.Pow(item.Value.z - itembis.Value.z, 2));
                    debug = debug + "La distance entre " + item.Key + " et " + itembis.Key + " est " + distanceBetweenItemAndItembis + "\n";
                    if (distanceBetweenItemAndItembis < 0.5)
                    {
                        cameraOffSet = item.Key + (Mathf.Acos(item.Value.z) * 180 / Mathf.PI);
                    }
                    score = score + distanceBetweenItemAndItembis;
                }
            }
            scores.Add(item.Key, score);
        }

        debug = debug + "Les trois scores sont :" + "\n";

        float keyWithHighestScore = 0;
        float highestScore = 0;
        foreach (KeyValuePair<float, float> itemtri in scores)
        {
            if (highestScore < itemtri.Value)
                keyWithHighestScore = itemtri.Key;
            debug = debug + itemtri.Value + "\n";
        }

        debug = debug + "La cle enlevee est " + keyWithHighestScore;
        dictionary.Remove(keyWithHighestScore);

        debugText.text = debug;
    }


    // Use this for initialization
    void Start () {
        cameraOffSet = 0;
        lastPosition = cameraPlayer.transform.position;
        guidage.SetActive(false);
        initialisationCanvas.SetActive(true);
        dictionary = new Dictionary<float, Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        if (cameraOffSet == 0)
        {
            if (dictionary.Count < 4)
            {
                AddValuesToDictionary();
            }
            else
            {
                TryToFindInterestingValue();
            }
        }
        else
        {
            initialisationCanvas.SetActive(false);
            guidage.SetActive(true);
        }
	}
}
