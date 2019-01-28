using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class BihouMovesAR : MonoBehaviour
{
    [SerializeField]
    private float distanceToTeleport;
    [SerializeField]
    private Interval directionChangeTimeInterval;
    [SerializeField]
    private Interval distanceToPlayerInterval;
    [SerializeField]
    private Interval bihouSpeedInterval;
    [SerializeField]
    private Camera cameraPlayer;

    private float directionChangeTime;
    [SerializeField]
    private float bihouSpeed;
    private float bihouDistance;

    private float latestDirectionChangeTime;
    private Vector3 movementDirection;
    private Vector3 movementPerSecond;

    private Vector3 targetPosition;
    [SerializeField]
    private float distToCamOrigin;
    [SerializeField]
    private float marginTarget;
    private float dirx = 0, diry = 0, dirz = 0;
    [SerializeField]
    private float coefDepth;

    private Vector3 targetPlane;
    public Vector3 TargetPlane
    {
        get { return targetPlane; }
        set { targetPlane = value; }
    }
    private bool isOnPlane = false;
    public bool IsOnPlane
    {
        get { return isOnPlane; }
        set{ isOnPlane = value; }
    }


    void Start()
    {
        targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(8 * Screen.width / 10, Screen.height / 2, distToCamOrigin));
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        latestDirectionChangeTime = 0.0f;
        CalcuateNewMovementVector();
    }



    void CalcuateNewMovementVector()
    {
        //CalcuateRandoms();

        //movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
        dirx = 0; diry = 0; dirz = 0;
        if (transform.position.x >= targetPosition.x + marginTarget) { dirx = -1; } if (transform.position.x <= targetPosition.x - marginTarget) { dirx = 1; }
        if (transform.position.y >= targetPosition.y + marginTarget) { diry = -1; } if (transform.position.y <= targetPosition.y - marginTarget) { diry = 1; }
        if (transform.position.z >= targetPosition.z + marginTarget) { dirz = -1*coefDepth; } if (transform.position.z <= targetPosition.z - marginTarget) { dirz = 1*coefDepth; }
        movementDirection = new Vector3(dirx, diry, dirz);
        movementPerSecond = movementDirection * bihouSpeed;
    }

    void CheckNotOutofBoundary()
    {
        float distance = Mathf.Sqrt(
            Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2)
            );
        if (distance >= distanceToPlayerInterval.max || distance <= distanceToPlayerInterval.min)
        {
            movementDirection = new Vector3(targetPosition.x - transform.position.x, 0.0f, targetPosition.z - transform.position.z).normalized;
            bihouSpeed = 2.0f;
            movementPerSecond = movementDirection * bihouSpeed;
        }
    }


    void TeleportForwardPlayer()
    {
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
    }

    public void CalculateTarget()
    {
        if(targetPlane != new Vector3(0, 0, 0))
        {
            if (isInCameraFieldOfView())
            {
                targetPosition = targetPlane;
                isOnPlane = true;
            }
            else
            {
                targetPlane = new Vector3(0, 0, 0);
                targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(7*Screen.width / 10, Screen.height / 2, distToCamOrigin));
                isOnPlane = false;
            }
        }
        else
        {
            targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(7*Screen.width / 10, Screen.height / 2, distToCamOrigin));
            isOnPlane = false;
        }
    }

    private bool isInCameraFieldOfView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraPlayer);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds); 
}



    void Update()
    {
        //Automatic raycasting 

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        //Raycast from the middle of the screen
        if (isOnPlane && Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                targetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    targetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    targetPlane = hit.Pose.position;
                }
            }

        }

        //Raycast from the left of the screen
        if (isOnPlane && Frame.Raycast(Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                targetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    targetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    targetPlane = hit.Pose.position;
                }
            }

        }

        //Raycast from the right of the screen
        if (isOnPlane && Frame.Raycast(3 * Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                targetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    targetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    targetPlane = hit.Pose.position;
                }
            }

        }

        CalculateTarget();
        transform.LookAt(cameraPlayer.transform);
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalcuateNewMovementVector();
        }

        CheckNotOutofBoundary();

        transform.position = transform.position + movementPerSecond * Time.deltaTime;

        float distanceBetweenBihouAndPlayer = Mathf.Sqrt(
        Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
        Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2));
        if (distanceToTeleport <= distanceBetweenBihouAndPlayer)
        {
            TeleportForwardPlayer();
        }

    }
}
