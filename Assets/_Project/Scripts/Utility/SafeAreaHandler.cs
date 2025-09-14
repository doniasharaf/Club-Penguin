using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Adjusts the layout of a UI element to fit within the screen's safe area, ensuring that content is not obscured by
    /// notches, rounded corners, or other display cutouts.
    /// </summary>
    /// <remarks>This component should be attached to a GameObject with a <see cref="RectTransform"/>. It
    /// automatically adjusts the anchors of the RectTransform to match the safe area of the screen. The safe area is
    /// determined using <see cref="Screen.safeArea"/> and is applied during the <c>Awake</c> lifecycle method.</remarks>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaHandler : MonoBehaviour
    {
        RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}