namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;


    //Controller class for the event scene
    public class Event_controller : MonoBehaviour
    {
        //The avatar
        [SerializeField]
        GameObject Bihou;

        //Text canvas for player notification
        [SerializeField]
        Text UItext_notification;

        //Text canvas for the number of current objectives done
        [SerializeField]
        Text UItext_objectiveNb;

        //Total number of objectives 
        [SerializeField]
        int nbObjective;


        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public Event_ImageVisualizer AugmentedImageVisualizerPrefab;

        //Dictionary of <image indexes, augmented object visualizer associated>
        private Dictionary<int, Event_ImageVisualizer> m_Visualizers = new Dictionary<int, Event_ImageVisualizer>();

        //List of augmented images
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        //Dictionary of <image indexes, their state (has been interacted with)>
        private Dictionary<int, bool> ImageObjectiveAchieved = new Dictionary<int, bool>();

        //Number of objectives done
        private int nbObjectiveDone = 0;

        //Canvas panel for the end of the scene
        public GameObject endGameCanvas;

        //Is it the end
        private bool endgame = false;

        //Timer between end of activity and activation of the ending panel
        private int timer = 0;



        // Use this for initialization
        void Start()
        {
            //Get the number of current objectives done via the AppController, and resets it if already max
            int globalNbObjectives = AppController.control.currentObjectiveDoneEvent;
            if(globalNbObjectives == nbObjective)
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
                //Displays the object if the poster is recognized and hasn't already been interacted with
                if (image.TrackingState == TrackingState.Tracking && visualizer == null && !alreadyDone)
                {
                    UItext_notification.text = "Affiche trouvée \nTouchez l'objet agumenté";
                    //Bihou lands to indicate the discovery of a poster
                    Bihou.GetComponent<AnimatorScript>().land = true;
                    ImageObjectiveAchieved.Add(image.DatabaseIndex, false);
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = (Event_ImageVisualizer)Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    m_Visualizers.Add(image.DatabaseIndex, visualizer);
                }
            }


            //Player touch manager
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
                        //If the player touches an augmented object
                        if (hit.collider.tag == "AugmentedObject")
                        {
                            foreach (KeyValuePair<int, Event_ImageVisualizer> pair in m_Visualizers)
                            {
                                //UItext_notification.text += "\n" + pair.Key + " " + pair.Value.gameObject.transform.GetChild(pair.Key).name + " " + hit.collider.gameObject.name;
                                //If the object is one of the displayed augmented object
                                if (pair.Value.gameObject.transform.GetChild(pair.Key).name == hit.collider.gameObject.name)
                                {
                                    //Checks if the object hasn't been seen already
                                    bool isAlreadyAchieved = false;
                                    ImageObjectiveAchieved.TryGetValue(pair.Key, out isAlreadyAchieved);
                                    if (!isAlreadyAchieved)
                                    {
                                        //Destroys the visualizer
                                        Destroy(hit.collider.gameObject);
                                        m_Visualizers.Remove(pair.Key);
                                        ImageObjectiveAchieved[pair.Key] = true;
                                        //Updates the number of objective done both in scene and in game 
                                        nbObjectiveDone++;
                                        AppController.control.currentObjectiveDoneEvent++;
                                        //Avatar's victory looping
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

            //Displays number of remaining objectives 
            UItext_objectiveNb.text = nbObjectiveDone.ToString() + " / " + nbObjective.ToString();

            //If all objectives cleared -> triggers endgame sequence
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

            //Endgame sequence
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
