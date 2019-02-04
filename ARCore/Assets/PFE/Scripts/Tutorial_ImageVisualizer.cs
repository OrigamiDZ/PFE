namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;


    //Visualizer class for the mission tutorial scene
    public class Tutorial_ImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// An object to place when an image is detected.
        /// </summary>
        public GameObject Egg;


        public void Update()
        {
            if ((Image == null || Image.TrackingState != TrackingState.Tracking))
            {
                Egg.SetActive(false);

                return;
            }

            Egg.SetActive(true);
        }
    }
}
