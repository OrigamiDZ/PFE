using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBetweenGameObjects : MonoBehaviour {

    public Material directionMaterial;
    private GameObject player;
    private GameObject target;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Target");

        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(directionMaterial);
        line.widthMultiplier = 0.5f;
        line.positionCount = 2;
    }
    // Update is called once per frame
    void Update () {
		if (target != null) {
            LineRenderer line = GetComponent<LineRenderer>();
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, target.transform.position);
        }
	}
}
