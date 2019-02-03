namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Collections;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    

    //Controller class for the mission minigame (hide and seek minigame)
    public class MissionMiniGame_Controller : MonoBehaviour
    {
        //Avatar
        [SerializeField]
        GameObject Bihou;

        //Player notification text
        [SerializeField]
        Text UItext_notification;

        //Offset for positionning augmented objects
        [SerializeField]
        float deltaEnvironmentToGround;

        //Timer before endgame action
        int timer = 0;

        //Has game ended
        bool endgame = false;

        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public MiniGameHnS_ImageVisualizer AugmentedImageVisualizerPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        ////Dictionary of <image indexes, augmented object visualizer associated>
        private Dictionary<int, MiniGameHnS_ImageVisualizer> m_Visualizers
           = new Dictionary<int, MiniGameHnS_ImageVisualizer>();

        //List of augmented images
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        //Environment anchor for augmented objects
        private Vector3 anchorEnvironment;

        //Is the augmented object anchored
        private bool anchored = false;

        //Augmented object visualizer
        private MiniGameHnS_ImageVisualizer visualizer = null;

        //Has the avatar been found
        private bool HnS_found = false;
    



        
        void Start()
        {
            UItext_notification.text = "Trouvez l'affiche cachée";
            FitToScanOverlay.SetActive(true);
        }
        


        void Update()
        {
            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);
            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            if (!anchored) {
                foreach (var image in m_TempAugmentedImages)
                {
                    m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                    if (image.TrackingState == TrackingState.Tracking && visualizer == null)
                    {
                        // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                        anchorEnvironment = image.CenterPose.position;
                        visualizer = (MiniGameHnS_ImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchorEnvironment + new Vector3(0,deltaEnvironmentToGround,0), Quaternion.identity);
                        UItext_notification.text = "Trouvez Bihou !";
                        FitToScanOverlay.SetActive(false);
                        anchored = true;
                        return;
                    }
                }
            }
            else
            {
                //Checks if player touches the avatar on screen
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
                            if (hit.collider.tag == "Bihou")
                            {
                                UItext_notification.text = "Bien joué !";
                                endgame = true;
                            }

                        }
                    }
                }
            }

            //Endgame sequence
            if (endgame)
            {
                timer++;
                if(timer > 250)
                {
                    AppController.control.missionDone = true;
                    SceneManager.LoadScene("Discovery");
                }
            }
        }

    }
}
