using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Location;
using UnityEngine.UI;

// this script was created in order to calculate the offset between the camera's forward vector and the vector heading towards north
public class GuidageController : MonoBehaviour {

    // first person camera
    public Camera cameraPlayer;

    // Vector of the last position, to calculate the direction's vector
    private Vector3 lastPosition;

    // dictionary with the offsets and the vector associated
    private Dictionary<float, Vector3> dictionary;

    // in order to not use OrderedDictionnary (because it's have a greater complexity while adding and removing object, the order the value got in
    private List<float> orderInDictionnary;

    // to get the location of the device  
    public DeviceLocationProvider deviceLocationProvider;

    // the list of camera offsets, to create a medium
    private List<float> cameraOffsets;

    // Bihou, the one and only
    public GameObject bihouPrefab;

    // calculate the medium form the list cameraOffsets
    public float GetCameraOffset()
    {
        float medium = 0;
        for (int i = 0; i < cameraOffsets.Count; i++)
        {
            medium = medium + cameraOffsets[i];
        }

        if (cameraOffsets.Count > 0)
            return medium / cameraOffsets.Count;
        else
            return medium;
    }

    // sort the camera offsets by removing the value that is the farest from the others
    private void SortCameraOffsets()
    {
        int farestValueIndex = 0;
        int highestDistance = 0;

        for (int i = 0; i < cameraOffsets.Count; i++)
        {
            float distance = 0;
            for (int j = 0; j < cameraOffsets.Count; j++)
            {
                if (i != j)
                {
                    distance = distance + Mathf.Abs(cameraOffsets[i] - cameraOffsets[j]);
                }
            }
            if (distance < highestDistance)
            {
                farestValueIndex = i;
            }
        }
        cameraOffsets.RemoveAt(farestValueIndex);

    }

    // add new value to the dictionnary : the calculated direction vector and the heading 
    void AddValuesToDictionary()
    {
        if (deviceLocationProvider.CurrentLocation.IsLocationServiceEnabled)
        {
            if (deviceLocationProvider.CurrentLocation.IsUserHeadingUpdated)
            {
                Vector3 directionVector = cameraPlayer.transform.position - lastPosition;
                if (directionVector.sqrMagnitude > 0.001f)
                {
                    dictionary.Add(deviceLocationProvider.CurrentLocation.UserHeading, Vector3.Normalize(directionVector));
                    orderInDictionnary.Add(deviceLocationProvider.CurrentLocation.UserHeading);
                }
                lastPosition = cameraPlayer.transform.position;
            }
        }
    }

    // try to find a value with the distance between two points being small enought
    void TryToFindInterestingValue()
    {
        foreach (KeyValuePair<float, Vector3> item in dictionary)
        {
            foreach (KeyValuePair<float, Vector3> itembis in dictionary)
            {
                if (item.Key != itembis.Key)
                {
                    // the 5 * is to give more weight to the distance between the vector than the heading value
                    float distanceBetweenItemAndItembis = Mathf.Abs(item.Key - itembis.Key) + 5 * Mathf.Sqrt( 
                        Mathf.Pow(item.Value.x - itembis.Value.x, 2) +
                        Mathf.Pow(item.Value.y - itembis.Value.y, 2) +
                        Mathf.Pow(item.Value.z - itembis.Value.z, 2));
                    if (distanceBetweenItemAndItembis < 0.5)
                    {
                        cameraOffsets.Add(item.Key + (Mathf.Acos(item.Value.z) * 180 / Mathf.PI));
                    }
                }
            }
        }
        dictionary.Remove(orderInDictionnary[0]);
        orderInDictionnary.RemoveAt(0);
    }


    // Use this for initialization
    void Start () {
        lastPosition = cameraPlayer.transform.position;
        bihouPrefab.SetActive(false);
        orderInDictionnary = new List<float>();
        cameraOffsets = new List<float>();
        dictionary = new Dictionary<float, Vector3>();
    }
	
	// Update is called once per frame
	void Update () {
        if (GetCameraOffset() != 0)
        {
            bihouPrefab.SetActive(true);
        }

        if (dictionary.Count < 4)
        {
            AddValuesToDictionary();
        }
        else
        {
            TryToFindInterestingValue();
        }

        if (cameraOffsets.Count > 20)
        {
            SortCameraOffsets();
        }
	}
}
