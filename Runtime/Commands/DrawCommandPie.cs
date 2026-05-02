using System;
using global::Shapes;
using UnityEngine;

namespace CupkekGames.Shapes
{
    public class DrawCommandPie : DrawCommand
    {
        [SerializeField] private ShapePie _shape;
        public override ShapeSequence CreateSequence()
        {
            return new ShapeSequence(_shape);
        }
    }
}