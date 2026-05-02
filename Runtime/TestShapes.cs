using global::Shapes;
using UnityEngine;

namespace CupkekGames.Shapes
{
    [ExecuteAlways]
    public class TestShapes : ImmediateModeShapeDrawer
    {
        public override void DrawShapes(Camera cam)
        {
            using (Draw.Command(cam))
            {
                // set up static parameters. these are used for all following Draw.Line calls
                Draw.LineGeometry = LineGeometry.Volumetric3D;
                Draw.ThicknessSpace = ThicknessSpace.Pixels;
                Draw.Thickness = 4; // 4px wide

                // set static parameter to draw in the local space of this object
                Draw.Matrix = transform.localToWorldMatrix;

                Draw.Pie(Vector3.zero, Quaternion.identity, 8, 0, 2, Color.green);
                Draw.Arc(Vector3.zero, Quaternion.identity, 8, 8, 0, 2, ArcEndCap.Round, Color.red);
            }
        }
    }
}