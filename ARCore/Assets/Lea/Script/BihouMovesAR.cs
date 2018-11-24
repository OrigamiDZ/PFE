using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interval
{
    public float min, max;
}

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


    void Start()
    {
        targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamOrigin));
        transform.position = targetPosition;
        transform.LookAt(cameraPlayer.transform);
        latestDirectionChangeTime = 0.0f;
        CalcuateNewMovementVector();
    }

    /*
    void CalcuateRandoms()
    {
        directionChangeTime = Random.Range(directionChangeTimeInterval.min, directionChangeTimeInterval.max);
        bihouSpeed = Random.Range(0.0f, bihouSpeedInterval.max);

        if (bihouSpeed < bihouSpeedInterval.min)
        {
            bihouSpeed = 0.0f;
        }
    }
    */

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

    /*
    private void OnCollisionEnter(Collision collision)
    {
        TeleportForwardPlayer();
        Debug.Log("Collision");
    }
    */

    void TeleportForwardPlayer()
    {
        //For now, teleports in front of camera, will later teleport just near it to come into screen
        //Vector3 newPosition = 
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
            }
            else
            {
                targetPlane = new Vector3(0, 0, 0);
                targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamOrigin));
            }
        }
        else
        {
            targetPosition = cameraPlayer.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distToCamOrigin));
        }
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
        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalcuateNewMovementVector();
        }

        CheckNotOutofBoundary();

        transform.position = transform.position + movementPerSecond * Time.deltaTime;

        float distanceBetweenBihouAndPlayer = Mathf.Sqrt(
    Mathf.Pow(cameraPlayer.transform.position.x - transform.position.x, 2) +
    Mathf.Pow(cameraPlayer.transform.position.z - transform.position.z, 2)
);
        if (distanceToTeleport <= distanceBetweenBihouAndPlayer)
        {
            TeleportForwardPlayer();
        }

    }
}
