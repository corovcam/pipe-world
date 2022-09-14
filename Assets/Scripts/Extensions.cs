using UnityEngine;

public static class RectTransformExtensions
{
    /// <summary>
    /// Rect Transform Extension for setting left position offset using relative position based on parent transform
    /// </summary>
    /// <param name="rectTransform">Transform to be updated</param>
    /// <param name="left">Left offset to move by</param>
    public static void SetLeft(this RectTransform rectTransform, float left)
    {
        rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
    }

    /// <summary>
    /// Rect Transform Extension for setting right position offset using relative position based on parent transform
    /// </summary>
    /// <param name="rectTransform">Transform to be updated</param>
    /// <param name="right">Right offset to move by</param>
    public static void SetRight(this RectTransform rectTransform, float right)
    {
        rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
    }

    /// <summary>
    /// Rect Transform Extension for setting top position offset using relative position based on parent transform
    /// </summary>
    /// <param name="rectTransform">Transform to be updated</param>
    /// <param name="top">Top offset to move by</param>
    public static void SetTop(this RectTransform rectTransform, float top)
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
    }

    /// <summary>
    /// Rect Transform Extension for setting bottom position offset using relative position based on parent transform
    /// </summary>
    /// <param name="rectTransform">Transform to be updated</param>
    /// <param name="bottom">Bottom offset to move by</param>
    public static void SetBottom(this RectTransform rectTransform, float bottom)
    {
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
    }
}
