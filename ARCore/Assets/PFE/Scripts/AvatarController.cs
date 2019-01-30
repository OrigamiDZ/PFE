using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;


[System.Serializable]
public class Interval
{
    public float min, max;
}

public class AvatarController : MonoBehaviour
{
    [SerializeField]
    private float distanceToTeleport;
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
    private bool isOnPlane = false;
    private bool planeInSight;
    private Vector3 basicPosition;

    [SerializeField]
    float landingVerticalOffset;
    
    private Vector3 landingOffsetVect;

    public Text debugText;



    void Start()
    {
        basicPosition = new Vector3(8 * Screen.width / 10, Screen.height / 2, distToCamOrigin);
        targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        landingOffsetVect = new Vector3(0, landingVerticalOffset, 0);
        latestDirectionChangeTime = 0.0f;
        CalcuateNewMovementVector();
    }



    void CalcuateNewMovementVector()
    {
        //movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
        dirx = 0; diry = 0; dirz = 0;
        if (transform.position.x >= targetPosition.x + marginTarget) { dirx = -1; }
        if (transform.position.x <= targetPosition.x - marginTarget) { dirx = 1; }
        if (transform.position.y >= targetPosition.y + marginTarget) { diry = -1; }
        if (transform.position.y <= targetPosition.y - marginTarget) { diry = 1; }
        if (transform.position.z >= targetPosition.z + marginTarget) { dirz = -1 * coefDepth; }
        if (transform.position.z <= targetPosition.z - marginTarget) { dirz = 1 * coefDepth; }
        movementDirection = new Vector3(dirx, diry, dirz);
        movementPerSecond = movementDirection * bihouSpeed;
    }

    void CheckNotOutofBoundary()
    {
        float distance = Mathf.Sqrt(
            Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2));
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
        if (AppController.control.inSpeechRecoMode)
        {
            targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 3, distToCamOrigin/1.1f));
            GetComponent<AnimatorScript>().takeoff = true;
        }
        else
        {
            if (planeInSight)
            {
                debugText.text = "Plane in sight";
                if (!isOnPlane)
                {
                    debugText.text = "Landing on plane";
                    targetPosition = targetPlane + landingOffsetVect;
                    GetComponent<AnimatorScript>().land = true;
                    isOnPlane = true;
                }
                else if (!isInCameraFieldOfView())
                {
                    debugText.text = "Not in sight so move yo ass";
                    targetPlane = new Vector3(0, 0, 0);
                    targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);
                    GetComponent<AnimatorScript>().takeoff = true;
                    isOnPlane = false;
                    planeInSight = false;
                }
            }
            else
            {
                debugText.text = "No plane in sight";
                if (isOnPlane)
                {
                    GetComponent<AnimatorScript>().takeoff = true;
                }
                targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);
                isOnPlane = false;
            }
        }
    }

    private bool isInCameraFieldOfView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraPlayer);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<CapsuleCollider>().bounds);
    }



    void Update()
    {
            //Automatic raycasting 

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;
            //Raycast from the middle of the screen
            if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) && Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                {
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
                        planeInSight = true;
                    }
                }

            }

            //Raycast from the left of the screen
            if (Frame.Raycast(Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
            {
                debugText.text = "Raycast left";
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) && Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                {
                    debugText.text = "Hit at back of the current DetectedPlane";
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
                        planeInSight = true;
                    }
                }

            }

            //Raycast from the right of the screen
            if (Frame.Raycast(3 * Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
            {
                debugText.text = "Raycast right";
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) && Vector3.Dot(cameraPlayer.transform.position - hit.Pose.position, hit.Pose.rotation * Vector3.up) < 0)
                {
                    debugText.text = "Hit at back of the current DetectedPlane";
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
                        planeInSight = true;
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
