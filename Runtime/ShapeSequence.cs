using System.Collections.Generic;

namespace CupkekGames.Shapes
{
    public class ShapeSequence
    {
        public List<Shape> Shapes = new List<Shape>();
        public ShapeSequence()
        {

        }
        public ShapeSequence(Shape shape)
        {
            Shapes.Add(shape);
        }
        public ShapeSequence(List<Shape> shapes)
        {
            Shapes = shapes;
        }
        public void Draw()
        {
            foreach (var shape in Shapes)
            {
                shape.Draw();
            }
        }
    }
}