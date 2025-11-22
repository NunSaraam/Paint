using UnityEngine;

namespace SceneScript
{
    public class CameraFollow2D : MonoBehaviour
    {
        [Header("Target to follow (the player)")]
        public Transform target;

        [Header("Camera offset (optional)")]
        public Vector3 offset = new Vector3(0, 0, -10);

        [Header("Follow smoothness speed")]
        public float smoothSpeed = 5f;

        // LateUpdate is used to ensure camera moves after all other updates (like player movement)
        void LateUpdate()
        {
            // If no target is assigned, do nothing
            if (target == null) return;

            // Calculate the desired camera position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly interpolate between current position and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
        }
    }
}
