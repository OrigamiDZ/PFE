using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.UI;


public class GuidageController : MonoBehaviour {

    private float cameraOffSet;
    public Camera cameraPlayer;
    private Vector3 lastPosition;

    public Text debugText1;
    public Text debugText2;


    private Dictionary<float, Vector3> dictionary;
    private List<float> orderInDictionnary;

    public DeviceLocationProvider deviceLocationProvider;

    private string debug2 = "";


    public GameObject bihouPrefab;
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
                debug2 = debug2 + "directionVector.sqrMagnitude = " + directionVector.sqrMagnitude + "\n";
                if (directionVector.sqrMagnitude > 0.001f)
                {
                    dictionary.Add(deviceLocationProvider.CurrentLocation.UserHeading, Vector3.Normalize(directionVector));
                    orderInDictionnary.Add(deviceLocationProvider.CurrentLocation.UserHeading);
                    debug2 = debug2 + "Vector added, " + dictionary.Count + " vectors in total" + "\n";
                }
                lastPosition = cameraPlayer.transform.position;
            }
        }
        debugText1.text = debug2;
    }

    void TryToFindInterestingValue()
    {
        string debug = "";
        foreach (KeyValuePair<float, Vector3> item in dictionary)
        {
            foreach (KeyValuePair<float, Vector3> itembis in dictionary)
            {
                if (item.Key != itembis.Key)
                {
                    float distanceBetweenItemAndItembis = Mathf.Abs(item.Key - itembis.Key) + 5 * Mathf.Sqrt(
                        Mathf.Pow(item.Value.x - itembis.Value.x, 2) +
                        Mathf.Pow(item.Value.y - itembis.Value.y, 2) +
                        Mathf.Pow(item.Value.z - itembis.Value.z, 2));
                    debug = debug + "La distance entre " + item.Key + " et " + itembis.Key + " est " + distanceBetweenItemAndItembis + " et les vecteur sont "+ item.Value + " et " + itembis.Value + "\n";
                    if (distanceBetweenItemAndItembis < 0.5)
                    {
                        cameraOffSet = item.Key + (Mathf.Acos(item.Value.z) * 180 / Mathf.PI);
                    }
                }
            }
        }
        dictionary.Remove(orderInDictionnary[0]);
        orderInDictionnary.RemoveAt(0);
        debugText2.text = debug;
    }


    // Use this for initialization
    void Start () {
        cameraOffSet = 0;
        lastPosition = cameraPlayer.transform.position;
        bihouPrefab.SetActive(false);
        initialisationCanvas.SetActive(true);
        orderInDictionnary = new List<float>();
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
            bihouPrefab.SetActive(true);
        }
	}
}
