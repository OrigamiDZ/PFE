using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSTestAure : MonoBehaviour {

    [SerializeField]
    private float inputLongTry; //lambda
    [SerializeField]
    private float inputLatTry; // phi

    

    Vector2 CalculEN(float inputLong, float inputLat)
    {
        Vector2 values = new Vector2(0, 0);

        int long0;
        int N0;
        float applatissement = 1 / 298.257223563f;
        float exentriciteCarre;
        float rayon = 6378137;
        const float k0 = 0.9996f;

        exentriciteCarre = 2 * applatissement - (applatissement * applatissement);

        float exentriciteP4 = exentriciteCarre * exentriciteCarre;
        float exentriciteP6 = exentriciteCarre * exentriciteCarre * exentriciteCarre;

        // calcul long0
        if (inputLong % 6 == 0)
            long0 = ((int)(inputLong / 6) - 1) * 6 + 3;
        else
            long0 = (int)(inputLong / 6) * 6 + 3;

        // calcul N0
        if (inputLat >= 0)
            N0 = 0;
        else
            N0 = 10000;

        float long0Rad = long0 * Mathf.PI / 180;
        float inputLongRad = Mathf.PI * inputLong / 180;
        float inputLatRad = Mathf.PI * inputLat / 180;

        float v = 1 / Mathf.Sqrt(1 - (exentriciteCarre * Mathf.Sin(inputLatRad) * Mathf.Sin(inputLatRad)));
        float A = (inputLongRad - long0Rad) * Mathf.Cos(inputLatRad);
        float s = (1 - (exentriciteCarre / 4) - (3 * exentriciteP4 / 64) - (5 * exentriciteP6 / 256)) * inputLatRad
            - ((3 * exentriciteCarre / 8) + (3 * exentriciteP4 / 32) + (45 * exentriciteP6 / 1024)) * Mathf.Sin(2 * inputLatRad)
            + ((15 * exentriciteP4 / 256) + (45 * exentriciteP6 / 1024)) * Mathf.Sin(4 * inputLatRad)
            - (35 * exentriciteP6 / 3072) * Mathf.Sin(6 * inputLatRad);
        float T = Mathf.Tan(inputLatRad) * Mathf.Tan(inputLatRad);
        float C = exentriciteCarre * Mathf.Cos(inputLatRad) * Mathf.Cos(inputLatRad) / (1 - exentriciteCarre);

        float E = 500 + ((k0 * rayon * v * (A + ((1 - T + C) * A * A * A / 6) + ((5 - (18 * T) + (T * T)) * A * A * A * A * A / 120))) / (10 * 10 * 10));

        float N = N0 + ((k0 * rayon * (s + (v * Mathf.Tan(inputLatRad)) * (A * A / 2 + (5 - T + 9 * C + (4 * C * C)) * A * A * A * A / 24 + (61 - 58 * T + T * T) * A * A * A * A * A * A / 720))) / (10 * 10 * 10));

        values.x = E;
        values.y = N;

        return values;
    }

	// Use this for initialization
	void Start () {
        
        float rapportX ;
        float rapportY;

    }
	
	// Update is called once per frame
	void Update () {

}
}
