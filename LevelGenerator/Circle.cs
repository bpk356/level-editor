using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LevelGenerator
{
    class Circle : IDraggableObject
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }

        public const int MutatableAttributeCount = Vector2.MutatableAttributeCount + 2;

        public Circle(Vector2 centerPoint, float radius)
        {
            Position = centerPoint;
            Radius = radius;
        }

        public Circle(Circle other)
        {
            Position = new Vector2(other.Position);
            Radius = other.Radius;
        }

        public Circle(string description)
        {
            string[] parts = description.Split(',');
            float positionX = float.Parse(parts[0]);
            float positionY = float.Parse(parts[1]);
            Position = new Vector2(positionX, positionY);
            Radius = float.Parse(parts[2]);
        }

        public bool IsVisible(Rect screenBounds)
        {
            if (!screenBounds.ContainsPoint(Position + Vector2.Left * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(Position + Vector2.Right * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(Position + Vector2.Up * Radius))
            {
                return false;
            }
            if (!screenBounds.ContainsPoint(Position + Vector2.Down * Radius))
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
                Position.MutateAttribute(targetAttribute);
            }
        }

        public static string StringDescription()
        {
            return "#PositionX,PositionY,Radius";
        }

        public override string ToString()
        {
            return Position.ToString() + "," + Radius;
        }

        public void Draw(Color color, Rect screenBounds)
        {
            DrawHelper.DrawCircle(Position, Radius, color, screenBounds);
        }

        #region IDraggableObject Members

        public bool MouseIsOnObject(Vector2 mousePosition)
        {
            return DistanceFromMouse(mousePosition) <= Radius;
        }

        public float DistanceFromMouse(Vector2 mousePosition)
        {
            return OffsetFromMouse(mousePosition).Magnitude();
        }

        public Vector2 OffsetFromMouse(Vector2 mousePosition)
        {
            return Position - mousePosition;
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            Position = new Vector2(newPosition);
        }

        #endregion
    }
}
