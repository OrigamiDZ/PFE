using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BihouGuidage : MonoBehaviour {

    [SerializeField]
    private GuidageController controller;
    [SerializeField]
    private float distanceToTeleport;
    [SerializeField]
    private Interval distanceToPlayerInterval;
    [SerializeField]
    private Interval bihouSpeedInterval;
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
    [SerializeField]
    private float directionChangeTime;


    private float dirx = 0, diry = 0, dirz = 0;

    private float BihouDistanceToPlayer; // we calculate it
    private float latestDirectionChangeTime;
    private Vector3 movementDirection;
    private Vector3 movementPerSecond;
    private Vector3 targetPosition;
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

    void CalculateNewMovementVector()
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
        transform.position = cameraPlayer.transform.position + cameraPlayer.transform.forward * 2;
        transform.LookAt(cameraPlayer.transform);
    }

    public void CalculateTarget()
    {
        float angle = ReturnMedium(GPS.GetAngle()) + controller.GetCameraOffset();
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.PI / 180), 0, Mathf.Sin(angle * Mathf.PI / 180));
        targetPosition = cameraPlayer.transform.position + direction * distanceToPlayerInterval.max;
    }

    public void AdjustSpeed()
    {
        if (Vector3.Distance(targetPosition, transform.position) >= 5 )
            if (bihouSpeed + 0.02f < bihouSpeedInterval.max)
                bihouSpeed += 0.01f;

        if (Vector3.Distance(targetPosition, transform.position) <= 2)
            if (bihouSpeed - 0.02f > bihouSpeedInterval.min)
                bihouSpeed -= 0.01f;
    }

    private float ReturnMedium(float gyrometre)
    {
        return (int)(gyrometre / 10) * 10.0f;
    }

    void CheckIfTargetReached()
    {
        if (Vector3.Distance(targetPosition, transform.position) < 0.1)
            reachedTarget = true;
        if (Vector3.Distance(targetPosition, transform.position) > 0.7)
            reachedTarget = false;
    }

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