using UnityEngine;
using global::Shapes;

namespace CupkekGames.Shapes
{
    [System.Serializable]
    public class ShapeDisc : Shape
    {
        public Vector3 Pivot;
        public Quaternion Rotation;
        public float Radius;
        public Color Color;
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
            global::Shapes.Draw.Disc(Pivot, Rotation, Radius, Color);
            global::Shapes.Draw.PopStyle();
        }
    }
}