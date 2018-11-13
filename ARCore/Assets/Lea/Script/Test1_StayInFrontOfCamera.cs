using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1_StayInFrontOfCamera : MonoBehaviour {

    public Camera FirstPersonCamera;

    public GameObject AndyPointPrefab;

    public float distToCamera;

    private const float k_ModelRotation = 180.0f;

    private GameObject objectInFrontCamera;

    private Vector3 positionObjectInFrontCamera;


    // Use this for initialization
    void Start () {
        FirstPersonCamera = Camera.main;
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamera));
        objectInFrontCamera = Instantiate(AndyPointPrefab, positionObjectInFrontCamera, Quaternion.identity);
        objectInFrontCamera.transform.LookAt(FirstPersonCamera.transform);
    }
	
	// Update is called once per frame
	void Update () {
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamera));
        objectInFrontCamera.transform.position = positionObjectInFrontCamera;
        objectInFrontCamera.transform.LookAt(FirstPersonCamera.transform);
    }
}
