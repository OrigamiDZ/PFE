using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;

public class ControllerGuidageParBihou : MonoBehaviour {
    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
    /// </summary>
    public Camera FirstPersonCamera;

    /// <summary>
    /// The rotation in degrees need to apply to model when the Andy model is placed.
    /// </summary>
    private const float k_ModelRotation = 180.0f;

    /// <summary>
    /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
    /// the application to avoid per-frame allocations.
    /// </summary>
    private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
    /// </summary>
    private bool m_IsQuitting = false;

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    /// 

    public GameObject Bihou;
    public void Update()
    {
        _UpdateApplicationLifecycle();

        // Hide snackbar when currently tracking at least one plane.
        Session.GetTrackables<DetectedPlane>(m_AllPlanes);

        //Automatic raycasting 

        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        //Raycast from the middle of the screen
        if (!Bihou.GetComponent<BihouMovesAR>().IsOnPlane && Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = hit.Pose.position;
                }
            }

        }

        //Raycast from the left of the screen
        if (!Bihou.GetComponent<BihouMovesAR>().IsOnPlane && Frame.Raycast(Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = hit.Pose.position;
                }
            }

        }

        //Raycast from the right of the screen
        if (!Bihou.GetComponent<BihouMovesAR>().IsOnPlane && Frame.Raycast(3 * Screen.width / 4, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
                Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
            }
            else
            {
                if (hit.Trackable is FeaturePoint)
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = new Vector3(0, 0, 0);
                }
                else
                {
                    Bihou.GetComponent<BihouMovesAR>().TargetPlane = hit.Pose.position;
                }
            }

        }
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
