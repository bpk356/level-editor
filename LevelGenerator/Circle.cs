using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LevelGenerator
{
    class Circle
    {
        public Vector2 CenterPoint { get; set; }
        public float Radius { get; set; }

        public const int MutatableAttributeCount = Vector2.MutatableAttributeCount + 2;

        public Circle(Vector2 centerPoint, float radius)
        {
            CenterPoint = centerPoint;
            Radius = radius;
        }

        public Circle(Circle other)
        {
            CenterPoint = new Vector2(other.CenterPoint);
            Radius = other.Radius;
        }

        public bool IsVisible(Rect screenBounds)
        {
            if (!screenBounds.ContainsPoint(CenterPoint + Vector2.Left * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(CenterPoint + Vector2.Right * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(CenterPoint + Vector2.Up * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(CenterPoint + Vector2.Down * Radius))
            {
                return false;
            }
            return true;
        }

        public void MutateAttribute(int targetAttribute)
        {
            if (targetAttribute == Vector2.MutatableAttributeCount)
            {
                Radius += 1.0f;
                return;
            }
            else if (targetAttribute == Vector2.MutatableAttributeCount + 1)
            {
                Radius -= 1.0f;
                return;
            }
            else
            {
                CenterPoint.MutateAttribute(targetAttribute);
            }
        }

        public override string ToString()
        {
            return "Center: " + CenterPoint.ToString() + ", Radius: " + Radius;
        }

        public void Draw(Color color, Rect screenBounds)
        {
            DrawHelper.DrawCircle(CenterPoint, Radius, color, screenBounds);
        }
    }
}
