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
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
                {
                    AugmentedObjectsList[Image.DatabaseIndex].SetActive(false);

                    return;
                }
                AugmentedObjectsList[Image.DatabaseIndex].SetActive(true);
            }
    }
}
