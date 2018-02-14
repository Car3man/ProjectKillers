using System;
using JetBrains.Annotations;
using UnityEngine;

public static class RectTransformExtensions {
    /// <summary>
    /// Returns true if the object is active and visible on the screen. Otherwise returns false.
    /// </summary>
    public static bool IsVisible([NotNull] this RectTransform rectTransform) {
        if (rectTransform == null) throw new ArgumentNullException("rectTransform");

        var screenRect = new Rect(0, 0, Screen.width, Screen.height);

        bool isActive = rectTransform.gameObject.activeInHierarchy;
        bool isWithinRect = IsWithinRect(rectTransform, screenRect);

        return isActive && isWithinRect;
    }

    public static bool IsWithinRect([NotNull] this RectTransform rectTransform, Rect rect) {
        if (rectTransform == null) throw new ArgumentNullException("rectTransform");

        var objectCorners = new Vector3[4];

        rectTransform.GetWorldCorners(objectCorners);

        bool allCornersWithinRect = true;

        foreach (Vector3 corner in objectCorners) {
            if (!rect.Contains(corner)) {
                allCornersWithinRect = false;
            }
        }

        return allCornersWithinRect;
    }

    public static Rect GetScreenSpaceRect([NotNull] this RectTransform transform) {
        if (transform == null) throw new ArgumentNullException("transform");

        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        float x = transform.position.x + transform.anchoredPosition.x;
        float y = Screen.height - transform.position.y - transform.anchoredPosition.y;

        return new Rect(x, y, size.x, size.y);
    }
}