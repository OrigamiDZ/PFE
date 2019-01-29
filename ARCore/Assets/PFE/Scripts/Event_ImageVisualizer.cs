namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class Event_ImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public List<AugmentedImage> AugmentedImagesList = new List<AugmentedImage>();

        /// <summary>
        /// An object to place when an image is detected.
        /// </summary>
        public GameObject[] AugmentedObject;

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            foreach (var image in AugmentedImagesList)
            {
                if (image == null || image.TrackingState != TrackingState.Tracking)
                {
                    AugmentedObject[image.DatabaseIndex].SetActive(false);

                    return;
                }


                AugmentedObject[image.DatabaseIndex].SetActive(true);
            }
        }
    }
}
