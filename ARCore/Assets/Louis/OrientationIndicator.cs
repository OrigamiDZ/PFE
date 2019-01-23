using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrientationIndicator : MonoBehaviour {

    //Choix de la caméra pour imposer l'angle de vue (qui correspond à l'appareil photo du tel)
    public Camera virtualCamera;

    //Affecter le text UI créer dans la scene 
    public Text TextNord;

    public GameObject northIndicator;
    public float distToCamOrigin = 2;

    //lance l'initialisation pour trouver le nord
    void Start() {
        Input.compass.enabled = true;
        Input.location.Start();

        Input.gyro.enabled = true;

        northIndicator.transform.position = virtualCamera.ScreenToWorldPoint(
            new Vector3(Screen.width / 2, Screen.height / 2, distToCamOrigin));


        northIndicator.transform.RotateAround(
            virtualCamera.transform.position,
            Input.gyro.gravity,
            Input.compass.trueHeading);
    }


    void Update() {
        //impose à la camera l'orientation dans la scene
        virtualCamera.transform.Rotate(0, Input.compass.trueHeading, 0);

        //affiche la valeur du nord mesurée par le compass du téléphone
        int XXX = (int)Input.compass.trueHeading;
        TextNord.text = XXX.ToString();
    }
}

