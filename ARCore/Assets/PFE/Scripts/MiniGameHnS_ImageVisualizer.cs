namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    //Visualizer class for the mission minigame scene
    public class MiniGameHnS_ImageVisualizer : MonoBehaviour
    {

        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// An object to place when an image is detected.
        /// </summary>
        public GameObject Forest;

        /// <summary>
        /// The Unity Update method.
        /// </summary>

        public void Start()
        {
            Forest.SetActive(true);
        }
    }
}
