using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_tuto : MonoBehaviour
{

    public Camera FirstPersonCamera;

    public float distToCamera;

    private const float k_ModelRotation = 180.0f;

    private Vector3 positionObjectInFrontCamera;


    // Use this for initialization
    void Start()
    {
        FirstPersonCamera = Camera.main;
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamera));
        transform.position = positionObjectInFrontCamera;
    }

    // Update is called once per frame
    void Update()
    {
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamera));
        transform.position = positionObjectInFrontCamera;
        transform.LookAt(FirstPersonCamera.transform);
    }
}
