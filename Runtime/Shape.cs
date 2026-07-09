using UnityEngine;
using global::Shapes;

namespace CupkekGames.Shapes
{
    [System.Serializable]
    public abstract class Shape
    {
        public LineGeometry LineGeometry = LineGeometry.Volumetric3D;
        public ThicknessSpace ThicknessSpace = ThicknessSpace.Pixels;
        public ShapesBlendMode BlendMode = ShapesBlendMode.Transparent;
        public Transform Transform;
        public abstract void Draw();
    }
}