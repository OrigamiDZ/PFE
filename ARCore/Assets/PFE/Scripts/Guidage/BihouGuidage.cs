using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BihouGuidage : MonoBehaviour {

    // controller which calculate the offset
    [SerializeField]
    private GuidageController controller;

    // distance minimum between player and Bihou for him to teleport
    [SerializeField]
    private float distanceToTeleport;

    // boundaries of distance between Bihou and the player
    [SerializeField]
    private Interval distanceToPlayerInterval;

    // bihou's speed interval
    [SerializeField]
    private Interval bihouSpeedInterval;

    // first person camera
    [SerializeField]
    private Camera cameraPlayer;

    // Bihou's speed
    [SerializeField]
    private float bihouSpeed;

    // distance to place Bihou when he's launched
    [SerializeField]
    private float distToCamOrigin;

    // margin for Bihou to decide if he has reached its target
    [SerializeField]
    private float marginTarget;

    // depth coefficient
    [SerializeField]
    private float coefDepth;

    // to get the angle direction
    [SerializeField]
    private GetDirectionFromMap GPS;

    // the time to update the direction vector
    [SerializeField]
    private float directionChangeTime;

    // current distance between the player camera and Bihou
    private float BihouDistanceToPlayer;

    // last time bihou changed direction
    private float latestDirectionChangeTime;

    // direction vector normalized
    private Vector3 movementDirection;

    // direction vector with speed
    private Vector3 movementPerSecond;

    // position targeted by bihou at any moment
    private Vector3 targetPosition;

    // if bihou has reached or not his target
    private bool reachedTarget;

    void Start()
    {
        targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(8 * Screen.width / 10, Screen.height / 2, distToCamOrigin));
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        latestDirectionChangeTime = 0.0f;
        reachedTarget = false;
        CalculateNewMovementVector();
    }

    // calculate the dircetion vector and the movement per seconds vector 
    void CalculateNewMovementVector()
    {
        float dirx = 0; float diry = 0; float dirz = 0;
        if (transform.position.x >= targetPosition.x + marginTarget) { dirx = -1; }
        if (transform.position.x <= targetPosition.x - marginTarget) { dirx = 1; }
        if (transform.position.y >= targetPosition.y + marginTarget) { diry = -1; }
        if (transform.position.y <= targetPosition.y - marginTarget) { diry = 1; }
        if (transform.position.z >= targetPosition.z + marginTarget) { dirz = -1 * coefDepth; }
        if (transform.position.z <= targetPosition.z - marginTarget) { dirz = 1 * coefDepth; }
        movementDirection = new Vector3(dirx, diry, dirz).normalized;
        movementPerSecond = movementDirection * bihouSpeed;
    }

    // check if Bihou is not out of boundaries
    void CheckNotOutofBoundary()
    {
        BihouDistanceToPlayer = Mathf.Sqrt(
            Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2)
            );
        if (BihouDistanceToPlayer >= distanceToPlayerInterval.max || BihouDistanceToPlayer <= distanceToPlayerInterval.min)
        {
            movementDirection = new Vector3(targetPosition.x - transform.position.x, 0.0f, targetPosition.z - transform.position.z).normalized;
            bihouSpeed = 2.0f;
            movementPerSecond = movementDirection * bihouSpeed;
        }
    }

    // teleport Bihou forward the player if he went too far away
    void TeleportForwardPlayer()
    {
        transform.position = cameraPlayer.transform.position + cameraPlayer.transform.forward * 2;
        transform.LookAt(cameraPlayer.transform);
    }

    // calculate the target in the space 
    public void CalculateTarget()
    {
        float angle = ReturnMedium(GPS.GetAngle() + controller.GetCameraOffset());
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.PI / 180), 0, Mathf.Sin(angle * Mathf.PI / 180));
        targetPosition = cameraPlayer.transform.position + direction * distanceToPlayerInterval.max;
    }

    // adjust speed according to the distance left from the final target point
    public void AdjustSpeed()
    {
        if (Vector3.Distance(targetPosition, transform.position) >= 5 )
            if (bihouSpeed + 0.02f < bihouSpeedInterval.max)
                bihouSpeed += 0.01f;

        if (Vector3.Distance(targetPosition, transform.position) <= 2)
            if (bihouSpeed - 0.02f > bihouSpeedInterval.min)
                bihouSpeed -= 0.01f;
    }

    // return the rounded power of 10 of the given value
    private float ReturnMedium(float gyrometre)
    {
        return (int)(gyrometre / 10) * 10.0f;
    }

    // check if Bihou target in the space is reached, to make it wait for us if it is
    void CheckIfTargetReached()
    {
        if (Vector3.Distance(targetPosition, transform.position) < 0.1)
            reachedTarget = true;
        if (Vector3.Distance(targetPosition, transform.position) > 0.7)
            reachedTarget = false;
    }

    // teleport if Bihou is too far
    void TeleportIfTooFar()
    {
        BihouDistanceToPlayer = Mathf.Sqrt(
        Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
        Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2)
    );
        if (distanceToTeleport <= BihouDistanceToPlayer)
        {
            TeleportForwardPlayer();
        }
    }

    void Update()
    {

        CalculateTarget();

        CheckIfTargetReached();

        if (reachedTarget)
        {
            transform.LookAt(cameraPlayer.transform);
        }
        else
        {
            transform.LookAt(targetPosition);

            AdjustSpeed();

            if (Time.time - latestDirectionChangeTime > directionChangeTime)
            {
                latestDirectionChangeTime = Time.time;
                CalculateNewMovementVector();
            }

            CheckNotOutofBoundary();

            transform.position = transform.position + movementPerSecond * Time.deltaTime;
        }

        TeleportIfTooFar();

    }
}