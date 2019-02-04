namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    //Visualizer class for the event minigame scene
    public class Event_ImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// An object to place when an image is detected.
        /// </summary>
        public GameObject[] AugmentedObjectsList;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            //Displays the augmented object only of the augmented image has been found
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
                {
                    AugmentedObjectsList[Image.DatabaseIndex].SetActive(false);

                    return;
                }
                AugmentedObjectsList[Image.DatabaseIndex].SetActive(true);
            }
    }
}
