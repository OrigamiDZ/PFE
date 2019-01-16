//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using System.Runtime.InteropServices;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    //using GoogleARCore.Examples.AugmentedImage;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class Tutorial_controller : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject TutorialTestUI;

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



        public GameObject Bihou;
        private string tutorialTestUIText;
        private int stepTestUI = 0;

        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public Tutorial_ImageVisualizer AugmentedImageVisualizerPrefab;
       
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        private Dictionary<int, Tutorial_ImageVisualizer> m_Visualizers
            = new Dictionary<int, Tutorial_ImageVisualizer>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        private bool flyerFound = false;


        private void Start()
        {
            tutorialTestUIText = "En recherche de plans...";
            stepTestUI = 1;
            Bihou.SetActive(false);
        }
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Step 1 : find a plan
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            if (stepTestUI == 1)
            {
                for (int i = 0; i < m_AllPlanes.Count; i++)
                {
                    if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                    {
                        tutorialTestUIText = "Plans trouvés !";
                        TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;
                        stepTestUI = 2;
                        break;
                    }
                }
            }



            //Step 2 : find the flyer
            if (stepTestUI == 2)
            {
                if (!flyerFound)
                {
                    tutorialTestUIText = "Scannez le flyer";
                    TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;
                    // Show the fit-to-scan overlay if there are no images that are Tracking.
                    foreach (var visualizer in m_Visualizers.Values)
                    {
                        if (visualizer.Image.TrackingState == TrackingState.Tracking)
                        {
                            FitToScanOverlay.SetActive(false);
                            return;
                        }
                    }

                    FitToScanOverlay.SetActive(true);
                }
                else
                {
                    tutorialTestUIText = "Scan effectué";
                    TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;
                    stepTestUI = 3;
                }

                foreach (var image in m_TempAugmentedImages)
                {
                    if (image.TrackingState == TrackingState.Tracking)
                    {
                        flyerFound = true;
                    }
                }

                // Check that motion tracking is tracking.
                if (Session.Status != SessionStatus.Tracking)
                {
                    Debug.Log("Session.Satus != Tracking");
                    return;
                }
            }



            //Step 3 : augment the flyer
            if (stepTestUI == 3)
            {
                // Get updated augmented images for this frame.
                Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);
                // Create visualizers and anchors for updated augmented images that are tracking and do not previously
                // have a visualizer. Remove visualizers for stopped images.
                foreach (var image in m_TempAugmentedImages)
                {
                    Tutorial_ImageVisualizer visualizer = null;
                    m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                    if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                    {
                        // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                        Anchor anchor = image.CreateAnchor(image.CenterPose);
                        visualizer = (Tutorial_ImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                        visualizer.Image = image;
                        m_Visualizers.Add(image.DatabaseIndex, visualizer);
                    }
                    else if (image.TrackingState == TrackingState.Stopped && visualizer != null)
                    {
                        m_Visualizers.Remove(image.DatabaseIndex);
                        GameObject.Destroy(visualizer.gameObject);
                        Debug.Log("Image tracking has stopped");
                        return;
                    }
                }
                tutorialTestUIText = "Touchez l'oeuf pour faire apparaître Bihou";
                TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;

                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }

                // Raycast against the location the player touched to search for planes.
                TrackableHit Hit;
                TrackableHitFlags RaycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                    TrackableHitFlags.FeaturePointWithSurfaceNormal;

                if (Frame.Raycast(touch.position.x, touch.position.y, RaycastFilter, out Hit))
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if (Hit.Trackable is AugmentedImage)
                    {
                        Bihou.SetActive(true);
                        Bihou.transform.position = Hit.Pose.position;
                        stepTestUI = 4;
                    }

                }
            }


            //Step 4 : Hello Bihou
            if(stepTestUI == 4)
            {
                tutorialTestUIText = "Bonjour Bihou !";
                TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;
                Debug.Log("Fin du tuto");
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
}
