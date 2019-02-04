using UnityEngine;

// this scripts prevent the mesh of the map, the player and all from being turned on
public class TurnOffRenderer : MonoBehaviour {

    private MeshRenderer[] meshs;

    void Update()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mesh in meshs)
            if (mesh.enabled == true)
                mesh.enabled = false;
    }

}
