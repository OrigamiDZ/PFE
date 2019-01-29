using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInFrontCamera : MonoBehaviour
{

    public Camera FirstPersonCamera;

    public float distToCamera;


    private Vector3 positionObjectInFrontCamera;




    void Update()
    {
        positionObjectInFrontCamera = FirstPersonCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 50, distToCamera));
        transform.position = positionObjectInFrontCamera;
        transform.LookAt(FirstPersonCamera.transform);
    }
}
