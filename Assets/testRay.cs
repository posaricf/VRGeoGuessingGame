namespace Mapbox.Examples
{
    using Mapbox.Unity.Utilities;
    using System;
    using UnityEngine;

    // C# example.

    public class testRay : MonoBehaviour
    {
        public QuadTreeCameraMovement qtcm;
        // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
        void FixedUpdate()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            //int layerMask = 1 << 6;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            // layerMask = ~layerMask;

            //RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            //{
            //    //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //    //Debug.Log("Did Hit");

            //    var rayPosition = hit.point;
            //    //print("HIT POINT: " + rayPosition);
            //    //var mousePosScreen = Input.mousePosition;
            //    //assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
            //    //http://answers.unity3d.com/answers/599100/view.html
            //    rayPosition.z = qtcm._referenceCamera.transform.localPosition.y;
            //    var pos = qtcm._referenceCamera.ScreenToWorldPoint(rayPosition);

            //    var latlongDelta = qtcm._mapManager.WorldToGeoPosition(pos);
            //    Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y + " RAY");

            //    //qtcm.HandleThumbstick();
            //    //Debug.Log("STRAIGHT OUTTA TESTRAY");
            //    /
            //    var zoom = Mathf.Max(0.0f, Mathf.Min(qtcm._mapManager.Zoom + qtcm.zoomAxis.y * qtcm._zoomSpeed, 21.0f));
            //    if (Math.Abs(zoom - qtcm._mapManager.Zoom) > 0.0f)
            //    {
            //        qtcm._mapManager.UpdateMap(qtcm._mapManager.CenterLatitudeLongitude, zoom);
            //    }

            //    if (Math.Abs(qtcm.thumbAxis.x) > 0.0f || Math.Abs(qtcm.thumbAxis.y) > 0.0f)
            //    {
            //         Get the number of degrees in a tile at the current zoom level.
            //         Divide it by the tile width in pixels ( 256 in our case)
            //         to get degrees represented by each pixel.
            //         Keyboard offset is in pixels, therefore multiply the factor with the offset to move the center.
            //        float factor = qtcm._panSpeed * (Conversions.GetTileScaleInDegrees((float)qtcm._mapManager.CenterLatitudeLongitude.x, qtcm._mapManager.AbsoluteZoom));

            //        var latitudeLongitude = new Utils.Vector2d(qtcm._mapManager.CenterLatitudeLongitude.x + qtcm.thumbAxis.y * factor * 2.0f, qtcm._mapManager.CenterLatitudeLongitude.y + qtcm.thumbAxis.x * factor * 4.0f);

            //        Debug.Log("LAT LONG: " + latitudeLongitude);
            //        qtcm._mapManager.UpdateMap(latitudeLongitude, qtcm._mapManager.Zoom);
            //    }
            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //    Debug.Log("Did not Hit");
            //}
        }
    }
}