using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interval
{
    public float min, max;
}

public class BihouMove : MonoBehaviour {
    [SerializeField]
    private float distanceToTeleport;
    [SerializeField]
    private Interval directionChangeTimeInterval;
    [SerializeField]
    private Interval distanceToPlayerInterval;
    [SerializeField]
    private Interval bihouSpeedInterval;
    [SerializeField]
    private GameObject player;

    private float directionChangeTime;
    private float bihouSpeed;
    private float bihouDistance;

    private float latestDirectionChangeTime;
    private Vector3 movementDirection;
    private Vector3 movementPerSecond;

    void Start()
    {
        latestDirectionChangeTime = 0.0f;
        CalcuateNewMovementVector();
    }

    void CalcuateRandoms()
    {
        directionChangeTime = Random.Range(directionChangeTimeInterval.min, directionChangeTimeInterval.max);
        bihouSpeed = Random.Range(0.0f, bihouSpeedInterval.max);

        if (bihouSpeed < bihouSpeedInterval.min)
        {
            bihouSpeed = 0.0f;
        }
    }

    void CalcuateNewMovementVector()
    {
        CalcuateRandoms();

        movementDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;
        movementPerSecond = movementDirection * bihouSpeed;
    }

    void CheckNotOutofBoundary()
    {
        float distance = Mathf.Sqrt(
            Mathf.Pow(player.transform.position.x - transform.position.x, 2) +
            Mathf.Pow(player.transform.position.z - transform.position.z, 2)
            );
        if (distance >= distanceToPlayerInterval.max || distance <= distanceToPlayerInterval.min)
        {
            movementDirection = new Vector3(player.transform.position.x - transform.position.x, 0.0f, player.transform.position.z - transform.position.z).normalized;
            bihouSpeed = 2.0f;
            movementPerSecond = movementDirection * bihouSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TeleportForwardPlayer();
        Debug.Log("Collision");
    }

    void TeleportForwardPlayer()
    {
        Vector3 newPosition = player.transform.position + player.transform.forward * 10.0f;
        transform.position = new Vector3(newPosition.x, 2.0f, newPosition.z);
    }

    void Update()
    {

        if (Time.time - latestDirectionChangeTime > directionChangeTime)
        {
            latestDirectionChangeTime = Time.time;
            CalcuateNewMovementVector();
        }

        CheckNotOutofBoundary();

        transform.position = transform.position + movementPerSecond * Time.deltaTime;

        float distanceBetweenBihouAndPlayer = Mathf.Sqrt(
    Mathf.Pow(player.transform.position.x - transform.position.x, 2) +
    Mathf.Pow(player.transform.position.z - transform.position.z, 2)
);
        if (distanceToTeleport <= distanceBetweenBihouAndPlayer)
        {
            TeleportForwardPlayer();
        }

    }
}
