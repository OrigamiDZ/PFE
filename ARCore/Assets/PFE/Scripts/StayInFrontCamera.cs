using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script that make the object it is attached to stay in front of the camera FirstPersonCamera
public class StayInFrontCamera : MonoBehaviour
{
    //Camera the object must stay in front of
    public Camera FirstPersonCamera;

    //Distance between camera and object
    public float distToCamera;

    //Position of object in front of camera
    private Vector3 positionObjectInFrontCamera;



    void Update()
    {
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 50, distToCamera));
        transform.position = positionObjectInFrontCamera;
        transform.LookAt(FirstPersonCamera.transform);
    }
}
