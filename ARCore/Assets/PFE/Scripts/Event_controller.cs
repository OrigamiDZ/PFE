﻿namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;
    public class Event_controller : MonoBehaviour
    {

        [SerializeField]
        GameObject Bihou;
        [SerializeField]
        Text UItext_notification;
        [SerializeField]
        Text UItext_objectiveNb;
        [SerializeField]
        int nbObjective;


        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public Event_ImageVisualizer AugmentedImageVisualizerPrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        //public GameObject FitToScanOverlay;

        private Dictionary<int, Event_ImageVisualizer> m_Visualizers = new Dictionary<int, Event_ImageVisualizer>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        private Dictionary<int, bool> ImageObjectiveAchieved = new Dictionary<int, bool>();
        //int : image.databaseIndex - bool : objectiveCompleted

        private int nbObjectiveDone = 0;

        public GameObject endGameCanvas;
        private bool endgame = false;
        private int timer = 0;



        // Use this for initialization
        void Start()
        {
            int globalNbObjectives = AppController.control.currentObjectiveDoneEvent;
            if(globalNbObjectives == 3)
            {
                nbObjectiveDone = 0;
                AppController.control.currentObjectiveDoneEvent = 0;
                AppController.control.eventDone = false;
            }
            else
            {
                nbObjectiveDone = AppController.control.currentObjectiveDoneEvent;
            }
            UItext_notification.text = "Trouvez les objets dans les affiches";
            UItext_objectiveNb.text = nbObjectiveDone.ToString() + " / " + nbObjective.ToString();
            UItext_objectiveNb.color = Color.red;
        }

        // Update is called once per frame
        void Update()
        {
            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);
            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            { 
                Event_ImageVisualizer visualizer = null;
                bool alreadyDone = false;
                m_Visualizers.TryGetValue(image.DatabaseIndex, out visualizer);
                ImageObjectiveAchieved.TryGetValue(image.DatabaseIndex, out alreadyDone);
                if (image.TrackingState == TrackingState.Tracking && visualizer == null && !alreadyDone)
                {
                    UItext_notification.text = "Affiche trouvée \nTouchez l'objet agumenté";
                    Bihou.GetComponent<AnimatorScript>().land = true;
                    ImageObjectiveAchieved.Add(image.DatabaseIndex, false);
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (Event_ImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    m_Visualizers.Add(image.DatabaseIndex, visualizer);
                }
            }

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
                        if (hit.collider.tag == "AugmentedObject")
                        {
                            foreach (KeyValuePair<int, Event_ImageVisualizer> pair in m_Visualizers)
                            {
                                UItext_notification.text = "foreach";
                                //UItext_notification.text += "\n" + pair.Key + " " + pair.Value.gameObject.transform.GetChild(pair.Key).name + " " + hit.collider.gameObject.name;
                                if (pair.Value.gameObject.transform.GetChild(pair.Key).name == hit.collider.gameObject.name)
                                {
                                    UItext_notification.text = "1e if";
                                    bool isAlreadyAchieved = false;
                                    ImageObjectiveAchieved.TryGetValue(pair.Key, out isAlreadyAchieved);
                                    if (!isAlreadyAchieved)
                                    {
                                        UItext_notification.text = "2e if";
                                        Destroy(hit.collider.gameObject);
                                        m_Visualizers.Remove(pair.Key);
                                        ImageObjectiveAchieved[pair.Key] = true;
                                        nbObjectiveDone++;
                                        AppController.control.currentObjectiveDoneEvent++;
                                        Bihou.GetComponent<AnimatorScript>().takeoff = true;
                                        Bihou.GetComponent<AnimatorScript>().looping = true;
                                        UItext_notification.text = "Trouvez les affiches restantes";
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            UItext_objectiveNb.text = nbObjectiveDone.ToString() + " / " + nbObjective.ToString();

            if (nbObjectiveDone == nbObjective)
            {
                UItext_objectiveNb.color = Color.green;
                UItext_notification.text = "Bien joué !";
                AppController.control.eventDone = true;
                endgame = true;
            }
            else
            {
                UItext_objectiveNb.color = Color.red;
            }


            if (endgame)
            {
                timer++;
                if(timer > 300)
                {
                    endGameCanvas.SetActive(true);
                }
            }

        }
    }
}
