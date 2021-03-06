﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBetweenGameObjects : MonoBehaviour {

    public Material directionMaterial;
    private GameObject player;
    private GameObject target;
    private float angle = 0;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Target");

        LineRenderer line = gameObject.AddComponent<LineRenderer>();
        line.material = new Material(directionMaterial);
        line.widthMultiplier = 0.8f;
        line.positionCount = 2;
        line.sortingOrder = 1;
    }

    public float GetAngle () {
        return 0;
    } 
    // Update is called once per frame
    void Update () {
		if (target != null) {
            LineRenderer line = GetComponent<LineRenderer>();
            line.SetPosition(0, player.transform.position + new Vector3(0, 1, 0));
            line.SetPosition(1, target.transform.position + new Vector3(0, 1, 0));
            float normLine = Mathf.Sqrt(
                Mathf.Pow((target.transform.position.z - player.transform.position.z), 2)
                + Mathf.Pow((target.transform.position.x - player.transform.position.x), 2));
            angle = Mathf.Acos(1 * (target.transform.position.z - player.transform.position.z) / normLine) * 180 / Mathf.PI;
            Debug.Log(angle);
        }
	}
}
