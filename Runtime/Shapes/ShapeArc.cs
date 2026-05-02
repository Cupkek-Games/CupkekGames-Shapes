using UnityEngine;
using global::Shapes;

namespace CupkekGames.Shapes
{
    [System.Serializable]
    public class ShapeArc : Shape
    {
        public Vector3 Pivot;
        public Quaternion Rotation;
        public float StartAngle;
        public float EndAngle;
        public float Radius;
        public Color Color;
        public float Thickness;
        public ArcEndCap ArcEndCap;
        public override void Draw()
        {
            global::Shapes.Draw.PushStyle();
            global::Shapes.Draw.LineGeometry = LineGeometry;
            global::Shapes.Draw.ThicknessSpace = ThicknessSpace;
            global::Shapes.Draw.BlendMode = BlendMode;
            if (Transform != null)
            {
                global::Shapes.Draw.Matrix = Transform.localToWorldMatrix;
            }
            global::Shapes.Draw.Arc(Pivot, Rotation, Radius, Thickness, StartAngle, EndAngle, ArcEndCap, Color);
            global::Shapes.Draw.PopStyle();
        }
    }
}