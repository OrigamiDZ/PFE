using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform playerTransform;
    private Vector3 cameraOffSet;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    // Use this for initialization
    void Start()
    {
        cameraOffSet = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = playerTransform.position + cameraOffSet;

        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
    }
}
