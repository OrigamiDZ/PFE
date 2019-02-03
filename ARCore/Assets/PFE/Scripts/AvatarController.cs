using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

//Interval class
[System.Serializable]
public class Interval
{
    public float min, max;
}


//Controller class for the avatar
public class AvatarController : MonoBehaviour
{
    //distance max between player and avatar before he teleports
    [SerializeField]
    private float distanceToTeleport;  

    //distance interval where avatar doesn't have to move
    [SerializeField]
    private Interval distanceToPlayerInterval;

    //first person camera used by player
    [SerializeField]
    private Camera cameraPlayer;

    //avatar's speed
    [SerializeField]
    private float bihouSpeed;

    //distance between first person camera and avatar's basic position
    [SerializeField]
    private float distToCamOrigin;

    //error margin for avatar to reach target
    [SerializeField]
    private float marginTarget;

    //depth coefficient for avatar's movement on z
    [SerializeField]
    private float coefDepth;

    //y offset for when avatar is landing
    [SerializeField]
    float landingVerticalOffset;

    //latest direction change time
    private float latestDirectionChangeTime;

    //direction of avatar's movement
    private Vector3 movementDirection;

    //avatar's movement per second
    private Vector3 movementPerSecond;

    //basic movement vectors
    private float dirx = 0, diry = 0, dirz = 0;

    //avatar's target position
    private Vector3 targetPosition;

    //avatar's target position when plane detected
    private Vector3 targetPlane;

    //has avatar landed on a plane
    private bool isOnPlane = false;

    //is a plane in sight of the camera
    private bool planeInSight;

    //default target position
    private Vector3 basicPosition;
    
    //offset vector for avatar's landing
    private Vector3 landingOffsetVect;

    //text canvas used for visual debugging
    public Text debugText;







    void Start()
    {
        //setting default position
        basicPosition = new Vector3(8 * Screen.width / 10, Screen.height / 2, distToCamOrigin);

        //setting first target position
        targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);

        //positionning avatar to his fist target
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        landingOffsetVect = new Vector3(0, landingVerticalOffset, 0);
        latestDirectionChangeTime = 0.0f;
        CalcuateNewMovementVector();
    }




    //Calculates next movement vector
    void CalcuateNewMovementVector()
    {
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




    //Checks if the avatar is in the correct range of the player perception
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




    //Teleports the avatar in front of the player
    void TeleportForwardPlayer()
    {
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
    }




    //Calculate next target for the avatar's movements
    public void CalculateTarget()
    {
        //If the game is in speech recognition mode -> the avatar is placed at the middle bottom of the screen
        if (AppController.control.inSpeechRecoMode)
        {
            targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 3, distToCamOrigin/1.1f));
            GetComponent<AnimatorScript>().takeoff = true;
        }

        //If the game is not in speech recognition mode -> either go for default postion or land on a plane
        else
        {
            //If a plane is spotted by the camera
            if (planeInSight)
            {
                debugText.text = "Plane in sight";
                //If the avatar is not currently on a plane, he lands
                if (!isOnPlane)
                {
                    debugText.text = "Landing on plane";
                    targetPosition = targetPlane + landingOffsetVect;
                    GetComponent<AnimatorScript>().land = true;
                    isOnPlane = true;
                }
                //Else if the avatar is not in sight of the player, he moves
                else if (!isInCameraFieldOfView())
                {
                    debugText.text = "Avatar not in sight, move";
                    targetPlane = new Vector3(0, 0, 0);
                    targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);
                    GetComponent<AnimatorScript>().takeoff = true;
                    isOnPlane = false;
                    planeInSight = false;
                }
                //Else the avatar doesn't move
            }

            //Else if there is no plane in sight, the avatar targets his default position
            else
            {
                debugText.text = "No plane in sight";
                //If the avatar is on a plane, he needs to take off first
                if (isOnPlane)
                {
                    GetComponent<AnimatorScript>().takeoff = true;
                }
                targetPosition = cameraPlayer.ScreenToWorldPoint(basicPosition);
                isOnPlane = false;
            }
        }
    }




    //Checks if the avatar is seen by the player
    private bool isInCameraFieldOfView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraPlayer);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<CapsuleCollider>().bounds);
    }





    void Update()
    {
            //Automatic raycasting from the left, the middle, and the right of the camera view to detect possible planes to land
            //When no plane is detected, the targetPlane is set to Vector3(0,0,0)

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



            //Calculates the new target for the avatar's movements
            CalculateTarget();

            //Asserts the avatar is always looking at the player
            transform.LookAt(cameraPlayer.transform);

            //Calculates the avatar's movements if enough time has passed
            if (Time.time - latestDirectionChangeTime > 0f)
            {
                latestDirectionChangeTime = Time.time;
                CalcuateNewMovementVector();
            }

            //Checks if the avatar is still in the correct boundaries
            CheckNotOutofBoundary();

            //Moves the avatar
            transform.position = transform.position + movementPerSecond * Time.deltaTime;

            //Calculates the distance between the player and the avatar, and teleports the avatar if it is too far apart
            float distanceBetweenBihouAndPlayer = Mathf.Sqrt(
            Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2));
            if (distanceToTeleport <= distanceBetweenBihouAndPlayer)
            {
                TeleportForwardPlayer();
            }
        
    }
}
