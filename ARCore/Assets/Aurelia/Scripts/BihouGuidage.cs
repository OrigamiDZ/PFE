using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BihouGuidage : MonoBehaviour {

    [SerializeField]
    private float distanceToTeleport;
    [SerializeField]
    private Interval distanceToPlayerInterval;
    [SerializeField]
    private Interval bihouSpeedInterval; // not used
    [SerializeField]
    private Camera cameraPlayer;
    [SerializeField]
    private float bihouSpeed;
    [SerializeField]
    private float distToCamOrigin;
    [SerializeField]
    private float marginTarget;
    [SerializeField]
    private float coefDepth;
    [SerializeField]
    private GetDirection GPS;

    private float dirx = 0, diry = 0, dirz = 0;

    private float BihouDistanceToPlayer; // we calculate it
    private float directionChangeTime;
    private float latestDirectionChangeTime;
    private Vector3 movementDirection;
    private Vector3 movementPerSecond;
    private Vector3 targetPosition;

    void Start()
    {
        targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(8 * Screen.width / 10, Screen.height / 2, distToCamOrigin));
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        latestDirectionChangeTime = 0.0f;
        CalculateNewMovementVector();
    }

    void CalculateNewMovementVector()
    {
        //CalcuateRandoms();

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

    void TeleportForwardPlayer()
    {
        //For now, teleports in front of camera, will later teleport just near it to come into screen
        //Vector3 newPosition = 
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
    }

    public void CalculateTarget()
    {
        float angle = GPS.GetAngle();
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

        targetPosition = cameraPlayer.transform.position + direction * distanceToPlayerInterval.max;


    }

    private float ReturnMedium(float gyrometre)
    {
        float medium = 0;
        if (gyrometre >= 342 || gyrometre < 18)
            medium = 0;
        else if (gyrometre >= 18 && gyrometre < 54)
            medium = 36;
        else if (gyrometre >= 54 && gyrometre < 90)
            medium = 72;
        else if (gyrometre >= 90 && gyrometre < 126)
            medium = 108;
        else if (gyrometre >= 126 && gyrometre < 162)
            medium = 144;
        else if (gyrometre >= 162 && gyrometre < 198)
            medium = 180;
        else if (gyrometre >= 198 && gyrometre < 234)
            medium = 216;
        else if (gyrometre >= 234 && gyrometre < 270)
            medium = 252;
        else if (gyrometre >= 270 && gyrometre < 306)
            medium = 288;
        else if (gyrometre >= 306 && gyrometre < 342)
            medium = 324;


        return medium;
    }

    private bool isInCameraFieldOfView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cameraPlayer);
        return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds);
    }

    void Update()
    {
        CalculateTarget();
        transform.LookAt(cameraPlayer.transform);
        transform.Rotate(new Vector3(-90, 0, 0));
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalculateNewMovementVector();
        }

        CheckNotOutofBoundary();

        transform.position = transform.position + movementPerSecond * Time.deltaTime;

        BihouDistanceToPlayer = Mathf.Sqrt(
    Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
    Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2)
);
        if (distanceToTeleport <= BihouDistanceToPlayer)
        {
            TeleportForwardPlayer();
        }

    }
}