using UnityEngine;

public class GetDirectionFromMap : MonoBehaviour {

    // player gameobject on the map
    public GameObject player;

    // target gameobject on the map
    public GameObject target;

    // first person camera
    public Camera cameraPlayer;

    // angle between the target and the player
    private float angle = 0;

    // getter for the angle var
    public float GetAngle()
    {
        return angle;
    }

    // calculate the angle between the target and the player
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
