using System;
using global::Shapes;
using UnityEngine;

namespace CupkekGames.Shapes
{
    public class DrawCommandArc : DrawCommand
    {
        [SerializeField] private ShapeArc _shape;
        public override ShapeSequence CreateSequence()
        {
            return new ShapeSequence(_shape);
        }
    }
}