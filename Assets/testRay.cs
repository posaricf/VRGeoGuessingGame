namespace Mapbox.Examples
{
    using UnityEngine;

    // C# example.

    public class testRay : MonoBehaviour
    {
        public QuadTreeCameraMovement qtcm;
        // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
        void FixedUpdate()
        {
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 6;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            // layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");

                var rayPosition = hit.point;
                //print("HIT POINT: " + rayPosition);
                //var mousePosScreen = Input.mousePosition;
                //assign distance of camera to ground plane to z, otherwise ScreenToWorldPoint() will always return the position of the camera
                //http://answers.unity3d.com/answers/599100/view.html
                rayPosition.z = qtcm._referenceCamera.transform.localPosition.y;
                var pos = qtcm._referenceCamera.ScreenToWorldPoint(rayPosition);

                var latlongDelta = qtcm._mapManager.WorldToGeoPosition(pos);
                //Debug.Log("Latitude: " + latlongDelta.x + " Longitude: " + latlongDelta.y + " RAY");
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }
}