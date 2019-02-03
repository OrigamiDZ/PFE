using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirection : MonoBehaviour {

    public GameObject player;
    public GameObject target;
    public Camera cameraPlayer;
    private float angle = 0;

    // Use this for initialization
    void Start()
    {

    }

    public float GetAngle()
    {
        return angle;
    }

    public void CalculateCameraOffSet()
    {

    }

    public void CalculateAngle()
    {
        float normLine = Mathf.Sqrt(
                    Mathf.Pow((target.transform.position.z - player.transform.position.z), 2)
                    + Mathf.Pow((target.transform.position.x - player.transform.position.x), 2));
        angle = Mathf.Acos(1 * (target.transform.position.z - player.transform.position.z) / normLine) * 180 / Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            CalculateAngle();
        }
    }
}
