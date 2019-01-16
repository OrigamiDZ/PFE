using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshBihou : MonoBehaviour {
    public Camera cam;
    public NavMeshAgent bihouNavMeshAgent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                bihouNavMeshAgent.SetDestination(hit.point);
            }
        }
    }

}
