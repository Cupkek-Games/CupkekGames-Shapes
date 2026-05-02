#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CupkekGames.Shapes
{
    [CustomEditor(typeof(PolylineSpline))]
    public class PolylineSplineEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            // Create the root element
            var root = new VisualElement();

            // Add the default inspector
            var defaultInspector = new IMGUIContainer(() => DrawDefaultInspector());
            root.Add(defaultInspector);

            Label title = new Label("PolylineSpline Debug");
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.style.fontSize = 24f;
            root.Add(title);

            PolylineSpline spline = (PolylineSpline)target;

            // Add buttons
            Button button = new Button(() => spline.Calculate())
            {
                text = "Recalculate"
            };
            root.Add(button);

            return root;
        }
    }
}
#endif