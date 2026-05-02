using UnityEngine;
using global::Shapes;
using System.Collections.Generic;

namespace CupkekGames.Shapes
{
    [RequireComponent(typeof(Polyline))]
    public class PolylineSpline : MonoBehaviour
    {
        private Polyline _polyline;
        public Polyline Polyline => _polyline;
        public Vector3 StartPosition;
        public Vector3 StartTangent;
        public Vector3 EndTangent;
        public Vector3 EndPosition;
        public int PointCount = 16;
        public float Thickness = 1f;
        public bool FixedThickness = true;
        public Gradient ColorGradient;

        private void Awake()
        {
            _polyline = GetComponent<Polyline>();
            _polyline.Geometry = PolylineGeometry.Billboard;
            _polyline.Closed = false;
        }

        public void Calculate()
        {
            _polyline.points = new List<PolylinePoint>(PointCount);

            for (int i = 0; i < PointCount; i++)
            {
                float normalizedValue = i / (PointCount - 1f);

                float thickness = FixedThickness ? Thickness : GetThickness(normalizedValue);

                _polyline.points.Add(new PolylinePoint(
                    GetBezierPt(StartPosition, StartTangent, EndTangent, EndPosition, normalizedValue),
                    GetColor(normalizedValue),
                    thickness
                ));
            }

            _polyline.meshOutOfDate = true;
        }

        static Vector3 GetBezierPt(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            float omt = 1f - t;
            float omt2 = omt * omt;
            return a * (omt2 * omt) + b * (3f * omt2 * t) + c * (3f * omt * t * t) + d * (t * t * t);
        }
        public Color GetColor(float normalizedValue)
        {
            // Clamp the value to ensure it stays between 0 and 1
            return ColorGradient.Evaluate(Mathf.Clamp01(normalizedValue));
        }
        public float GetThickness(float normalizedValue)
        {
            // Clamp the value to ensure it stays between 0 and 1
            return Mathf.Max(0.1f, Thickness * Mathf.Clamp01(normalizedValue));
        }
        public void SetGradient(Gradient gradient)
        {
            ColorGradient = gradient;
        }
        public void Clear()
        {
            _polyline.points.Clear();
            _polyline.meshOutOfDate = true;
        }
    }
}