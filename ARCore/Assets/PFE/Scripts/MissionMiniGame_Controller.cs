namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Collections;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class MissionMiniGame_Controller : MonoBehaviour
    {

        [SerializeField]
        GameObject Bihou;
        [SerializeField]
        Text UItext_notification;
        [SerializeField]
        float deltaEnvironmentToGround;
        int timer = 0;
        bool endgame = false;

        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public MiniGameHnS_ImageVisualizer AugmentedImageVisualizerPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;
        private Dictionary<int, MiniGameHnS_ImageVisualizer> m_Visualizers
           = new Dictionary<int, MiniGameHnS_ImageVisualizer>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        private Vector3 anchorEnvironment;
        private bool anchored = false;
        private MiniGameHnS_ImageVisualizer visualizer = null;
        private bool HnS_found = false;
    


        // Use this for initialization
        void Start()
        {
            UItext_notification.text = "Trouvez l'affiche cachée";
            FitToScanOverlay.SetActive(true);
        }

        // Update is called once per frame
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
