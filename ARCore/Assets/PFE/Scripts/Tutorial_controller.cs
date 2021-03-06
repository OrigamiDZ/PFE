﻿namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using System.Collections;



    //Controller class for the tutorial scene
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
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        //Avatar
        public GameObject Bihou;

        //Text canvas for player notification
        private string tutorialTestUIText;

        //ID of the tutorial step
        private int stepTestUI = 0;

        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public Tutorial_ImageVisualizer AugmentedImageVisualizerPrefab;
       
        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        //Dictionary of <image indexes, augmented object visualizer associated>
        private Dictionary<int, Tutorial_ImageVisualizer> m_Visualizers
            = new Dictionary<int, Tutorial_ImageVisualizer>();

        //List of augmented images
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        //Is the flyer found
        private bool flyerFound = false;

        //Timer for the UI delay
        private int timerUI = 0;

        //Anchor for egg augmented object
        private Anchor anchorEgg;

        //Timer for switching scene
        private int timerSwitch = 0;


        //Point cloud visualizer
        [SerializeField]
        GameObject pointsCloud;

        //Plane visualizer
        [SerializeField]
        GameObject planesGenerator;

        //Tutorial explanation slides
        [SerializeField]
        GameObject UI_slides;

        //Prefab for speech recognition 
        [SerializeField]
        GameObject speechRecoPrefab;

        //Offset for avatar spawning position
        [SerializeField]
        Vector3 offsetBihou;

        //Button for lost flyer
        [SerializeField]
        GameObject flyerLostButton;


        private void Start()
        {
            tutorialTestUIText = "Scannez autour de vous pour trouver des plans";
            stepTestUI = 1;
            Bihou.SetActive(false);
            UI_slides.SetActive(false);
            FitToScanOverlay.SetActive(false);
            flyerLostButton.SetActive(false);
        }
        public void Update()
        {
            _UpdateApplicationLifecycle();
            TutorialTestUI.GetComponent<Text>().text = tutorialTestUIText;

            // Step 1 : find a plane
            if (stepTestUI == 1)
            {
                Session.GetTrackables<DetectedPlane>(m_AllPlanes);
                for (int i = 0; i < m_AllPlanes.Count; i++)
                {
                    if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                    {
                        tutorialTestUIText = "Plans trouvés !";
                        timerUI++;
                        if (timerUI > 100)
                        {
                            stepTestUI = 2;
                        }
                        break;
                    }
                }
            }



            //Step 2 : find the flyer
            if (stepTestUI >= 2 && stepTestUI < 4)
            {
                planesGenerator.SetActive(false);
                pointsCloud.SetActive(false);

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
                        anchorEgg = anchor;
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


                //Step 2 : scan the flyer)

                if (stepTestUI == 2)
                {
                    if (!flyerFound)
                    {
                        tutorialTestUIText = "Scannez le flyer";
                        StartCoroutine("waiterButtonFlyerLost");

                        // Show the fit-to-scan overlay if there are no images that are Tracking.
                        foreach (var visualizer in m_Visualizers.Values)
                        {
                            if (visualizer.Image.TrackingState == TrackingState.Tracking)
                            {
                                flyerFound = true;
                                return;
                            }
                        }
                        FitToScanOverlay.SetActive(true);

                    }
                    else
                    {
                        tutorialTestUIText = "Scan effectué";
                        FitToScanOverlay.SetActive(false);
                        stepTestUI = 3;
                    }

                    // Check that motion tracking is tracking.
                    if (Session.Status != SessionStatus.Tracking)
                    {
                        tutorialTestUIText = "Session track pas :(";
                        Debug.Log("Session.Satus != Tracking");
                        return;
                    }
                }


                //Step 3 : augment the flyer
                if (stepTestUI == 3)
                {
                    flyerLostButton.SetActive(false);
                    tutorialTestUIText = "Touchez l'oeuf pour faire apparaître Bihou";
                    Touch touch;
                    if (Input.touchCount > 0)
                    {
                        touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Began)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(touch.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit))
                            {
                                if (hit.collider.tag == "Tuto_AugmentedObject")
                                {
                                    Bihou.SetActive(true);
                                    Bihou.GetComponent<AnimatorScript>().land = true;
                                    Bihou.transform.position = hit.point + offsetBihou;
                                    Bihou.transform.LookAt(FirstPersonCamera.transform);
                                    Bihou.transform.eulerAngles -= new Vector3(Bihou.transform.eulerAngles.x, 0, 0);
                                    Destroy(hit.transform.gameObject);
                                    stepTestUI = 4;
                                }

                            }
                        }
                    }
                }


            }
   


            //Step 4 : ???
            if(stepTestUI == 4)
            {
                planesGenerator.SetActive(false);
                pointsCloud.SetActive(false);
                Bihou.transform.LookAt(FirstPersonCamera.transform);
                Bihou.transform.eulerAngles -= new Vector3(Bihou.transform.eulerAngles.x, 0, 0);
                tutorialTestUIText = "Appuyez sur le bouton et dites \"bonjour\" à Bihou!";
                stepTestUI = 5;
            }

            //Step 5 : profit
            if(stepTestUI == 5)
            {
                speechRecoPrefab.SetActive(true);
                Bihou.transform.LookAt(FirstPersonCamera.transform);
                Bihou.transform.eulerAngles -= new Vector3(Bihou.transform.eulerAngles.x, 0, 0);
                if (speechRecoPrefab.GetComponent<Tutorial_SpeechRecognizer>().hello == true)
                {
                    tutorialTestUIText = "Bihou est content";
                    Bihou.GetComponent<AnimatorScript>().takeoff = true;
                    Bihou.GetComponent<AnimatorScript>().looping = true;
                    AppController.control.tutorialDone = true;
                    timerSwitch++;
                    if (timerSwitch > 200)
                    {
                        SceneManager.LoadScene("Discovery");
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


        //Waiter for button 
        IEnumerator waiterButtonFlyerLost()
        {
            yield return new WaitForSeconds(5);
            if(stepTestUI == 2)
            {
                flyerLostButton.SetActive(true);
            }
        }

        //I lost my flyer button function
        public void OnClickILostMyFlyer()
        {
            Vector3 defaultPos = new Vector3(Screen.width / 2, Screen.height / 2, 0.7f);
            Vector3 targetPos = FirstPersonCamera.ScreenToWorldPoint(defaultPos);
            Bihou.SetActive(true);
            Bihou.GetComponent<AnimatorScript>().land = true;
            Bihou.transform.position = targetPos;
            Bihou.transform.LookAt(FirstPersonCamera.transform);
            Bihou.transform.eulerAngles -= new Vector3(Bihou.transform.eulerAngles.x, 0, 0);
            FitToScanOverlay.SetActive(false);
            flyerLostButton.SetActive(false);
            stepTestUI = 4;
        }
    }
}
