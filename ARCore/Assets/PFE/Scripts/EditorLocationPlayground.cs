namespace Mapbox.Unity.Location {
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Mapbox.Unity.Utilities;
    using Mapbox.Utils;
    using UnityEngine;

    public class EditorLocationPlayground : AbstractEditorLocationProvider {

        public double initLatitude;
        public double initLongitude;
        /// <summary>
        /// The mock "latitude, longitude" location, respresented with a string.
        /// You can search for a place using the embedded "Search" button in the inspector.
        /// This value can be changed at runtime in the inspector.
        /// </summary>
        [SerializeField]

        void Start() {
            _currentLocation.IsLocationServiceEnabled = true;
            _currentLocation.IsLocationServiceInitializing = false;
            _currentLocation.IsLocationUpdated = true;
            _currentLocation.LatitudeLongitude = new Vector2d(initLatitude, initLongitude);
            _currentLocation.Accuracy = 34;
            _currentLocation.UserHeading = 304.5F;
            _currentLocation.DeviceOrientation = 304.5F;


    }


        protected override void SetLocation() {
            float verticalInput = Input.GetAxis("Vertical");
            float HorizontalInput = Input.GetAxis("Horizontal");
            if (verticalInput != 0) {
                _currentLocation.LatitudeLongitude = new Vector2d(
                    _currentLocation.LatitudeLongitude.x + 0.0001 * verticalInput,
                    _currentLocation.LatitudeLongitude.y);
            }
            if (HorizontalInput != 0) {
                _currentLocation.LatitudeLongitude = new Vector2d(
                    _currentLocation.LatitudeLongitude.x,
                    _currentLocation.LatitudeLongitude.y + 0.0001 * HorizontalInput);
            }

            // no need to check if 'MoveNext()' returns false as LocationLogReader loops through log file

        }
    }
}
