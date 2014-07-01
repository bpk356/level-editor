using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelGenerator
{
    class Rect
    {
        public float MaxX { get; set; }
        public float MinX { get; set; }
        public float MaxY { get; set; }
        public float MinY { get; set; }

        public float Width { get { return MaxX - MinX; } }
        public float Height { get { return MaxY - MinY; } }

        public Rect(float minX, float minY, float maxX, float maxY)
        {
            MinX = minX;
            MinY = minY;
            MaxX = maxX;
            MaxY = maxY;
        }

        public bool ContainsPoint(Vector2 point)
        {
            return point.X < MaxX && point.X > MinX && point.Y > MinY && point.Y < MaxY;
        }
    }
}
