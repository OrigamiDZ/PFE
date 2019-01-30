using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffRenderer : MonoBehaviour {

    public MeshRenderer[] meshs;

    void Update()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshs)
            if (mesh.enabled == true)
                mesh.enabled = false;
    }

}
