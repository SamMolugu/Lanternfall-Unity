using UnityEngine;

namespace Lanternfall.Core
{
    /// <summary>Keeps orthographic framing cinematic as window resizes.</summary>
    [RequireComponent(typeof(Camera))]
    public class CinematicOrthoCamera : MonoBehaviour
    {
        [SerializeField] float targetHeight = 3.2f;
        [SerializeField] float minSize = 2.6f;
        [SerializeField] float maxSize = 4.2f;

        Camera cam;

        void Awake() => cam = GetComponent<Camera>();

        void LateUpdate()
        {
            if (!cam.orthographic) return;
            var aspect = (float)Screen.width / Mathf.Max(1, Screen.height);
            // Wider screens pull back slightly so the cave wings stay in frame
            var size = targetHeight * Mathf.Lerp(1.05f, 0.92f, Mathf.InverseLerp(0.9f, 1.8f, aspect));
            cam.orthographicSize = Mathf.Clamp(size, minSize, maxSize);
        }
    }
}
