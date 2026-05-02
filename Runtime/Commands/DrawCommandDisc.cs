using System;
using global::Shapes;
using UnityEngine;

namespace CupkekGames.Shapes
{
    public class DrawCommandDisc : DrawCommand
    {
        [SerializeField] private ShapeDisc _shape;
        public override ShapeSequence CreateSequence()
        {
            return new ShapeSequence(_shape);
        }
    }
}