---
layout: default
---

## [Extensions.cs](https://github.com/corovcam/pipe-world/blob/main/Assets/Scripts/StaticClasses/Extensions.cs)

The `RectTransformExtensions` static class provides extension methods for Unity's `RectTransform` class, allowing developers to easily modify the position offsets of a `RectTransform` object relative to its parent transform. This can be particularly useful when working with UI elements in a Canvas, where positioning is often based on the parent element.

The class has four extension methods:

1. `SetLeft()`: Sets the left offset of the `RectTransform` relative to its parent.
2. `SetRight()`: Sets the right offset of the `RectTransform` relative to its parent.
3. `SetTop()`: Sets the top offset of the `RectTransform` relative to its parent.
4. `SetBottom()`: Sets the bottom offset of the `RectTransform` relative to its parent.

Each method accepts a `RectTransform` instance and a float value representing the desired offset for the respective edge. The methods update the `offsetMin` and `offsetMax` properties of the `RectTransform` to apply the specified offset.

For example, in the pipe-world project, if you have a `RectTransform` object representing a UI panel and you want to position it 10 units from the left edge and 5 units from the top edge of its parent, you can call the extension methods like this:

```csharp
RectTransform panel = ...; // Your RectTransform object
panel.SetLeft(10f);
panel.SetTop(5f);
```

These extension methods facilitate precise positioning of UI elements within a Unity project, making it easier to create and maintain responsive layouts.

### Questions & Answers

1. **How do these extension methods handle negative offset values?**

   The methods accept negative values without any restrictions, allowing developers to position the `RectTransform` either inside or outside the parent's bounds.

2. **Can these extension methods be used to simultaneously set offsets for multiple edges?**

   Yes, developers can chain multiple method calls to set offsets for multiple edges, e.g., `panel.SetLeft(10f).SetTop(5f);`.

3. **Would these methods work with non-UI elements, such as regular `Transform` objects?**

   No, these methods are specifically designed for `RectTransform` objects, which are used for UI elements in Unity. They won't work with regular `Transform` objects.
