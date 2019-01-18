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

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                Egg.SetActive(false);

                return;
            }

            //float halfWidth = Image.ExtentX / 2;
            //float halfHeight = Image.ExtentZ / 2;
            //Egg.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight *Vector3.back);

            Egg.SetActive(true);
        }
    }
}
