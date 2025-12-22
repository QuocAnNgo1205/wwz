using UnityEngine;

namespace CoopZombieShooter.Player
{
    /// <summary>
    /// Simple third-person camera follow script.
    /// Use this as an alternative to Cinemachine for quick testing.
    /// Attach this to Main Camera.
    /// </summary>
    public class SimpleCameraFollow : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0, 3, -6);
        [SerializeField] private float smoothSpeed = 10f;
        [SerializeField] private float lookAtHeight = 1.5f;
        [SerializeField] private bool autoFindTarget = true;

        [Header("Manual Target (Optional)")]
        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            // Auto-find target if enabled and not set
            if (autoFindTarget && target == null)
            {
                FindLocalPlayerTarget();
            }

            if (target == null) return;

            // Calculate desired position
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move camera
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            // Look at target
            Vector3 lookAtPoint = target.position + Vector3.up * lookAtHeight;
            transform.LookAt(lookAtPoint);
        }

        private void FindLocalPlayerTarget()
        {
            // Find all PlayerControllers
            var players = FindObjectsOfType<PlayerController>();

            foreach (var player in players)
            {
                // Check if this is the local player
                if (player.IsOwner)
                {
                    // Get the camera target from the player
                    Transform cameraTarget = player.transform.Find("CameraTarget");
                    if (cameraTarget != null)
                    {
                        target = cameraTarget;
                        Debug.Log($"Camera found local player target: {target.name}");
                        break;
                    }
                    else
                    {
                        // If no camera target, use player root
                        target = player.transform;
                        Debug.Log($"Camera following player root: {target.name}");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Manually set the target (useful for cutscenes or manual control)
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            autoFindTarget = false;
        }

        /// <summary>
        /// Clear target and re-enable auto-find
        /// </summary>
        public void ClearTarget()
        {
            target = null;
            autoFindTarget = true;
        }

        // Draw gizmo in editor
        private void OnDrawGizmosSelected()
        {
            if (target == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawWireSphere(target.position + Vector3.up * lookAtHeight, 0.3f);
        }
    }
}