using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LevelGenerator
{
    class SpaceBody : IDraggableObject
    {
        public const float GravityForce = 40000.0f;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Radius { get; set; }
        public float Mass 
        {
            get
            {
                return (float)Math.Pow(Radius, 3.0) * 0.5f;
            }
            set
            {
                Radius = (float)Math.Pow(value / 0.5, 1.0 / 3.0);
            }
        }
        public bool IsStationary { get; set; }
        public bool IsDestroyed { get; set; }

        public const int MutatableAttributeCount = Vector2.MutatableAttributeCount * 2 + 2;

        public SpaceBody(Random generator, Rect bounds)
        {
            Position = new Vector2((float)generator.NextDouble() * bounds.Width + bounds.MinX, (float)generator.NextDouble() * bounds.Height + bounds.MinY);
            Radius = generator.Next(10) + 4;
            do
            {
                Velocity = new Vector2((float)generator.NextDouble() * bounds.Width + bounds.MinX, (float)generator.NextDouble() * bounds.Height + bounds.MinY);
            } while (VelocityWillGoOffscreen(bounds));
            Velocity = new Vector2(0, 0);
            IsStationary = false;
            IsDestroyed = false;
        }

        public SpaceBody(Vector2 position, Vector2 velocity, int radius)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            IsDestroyed = false;
        }

        public SpaceBody(string description)
        {
            string[] parts = description.Split(',');
            float positionX = float.Parse(parts[0]);
            float positionY = float.Parse(parts[1]);
            Position = new Vector2(positionX, positionY);
            float velocityX = float.Parse(parts[2]);
            float velocityY = float.Parse(parts[3]);
            Velocity = new Vector2(velocityX, velocityY);
            Radius = int.Parse(parts[4]);
            IsStationary = bool.Parse(parts[5]);
            IsDestroyed = false;
        }

        public void Draw(Color color, bool drawVelocity, Rect screenBounds)
        {
            if (IsDestroyed)
            {
                return;
            }
            if (Radius == 0)
            {
                return;
            }
            DrawHelper.DrawCircle(Position, Radius, color, screenBounds);
            if (drawVelocity)
            {
                DrawHelper.DrawVelocity(Position, Velocity, color, screenBounds);
            }
        }

        public static string StringDescription()
        {
            return "#PositionX,PositionY,VelocityX,VelocityY,Radius,IsStationary";
        }

        public override string ToString()
        {
            return Position.ToString() + "," + Velocity.ToString() + "," + Radius + "," + IsStationary;
        }

        public bool IsVisible(Rect bounds)
        {
            return bounds.ContainsPoint(Position);
        }

        public void UpdateVelocity(IEnumerable<SpaceBody> allBodies, float deltaTime)
        {
            if (IsDestroyed)
            {
                return;
            }
            if (IsStationary)
            {
                return;
            }
            Vector2 totalForce = new Vector2(0, 0);
            int bodyCount = 0;

            foreach (var body in allBodies)
            {
                if (body == this)
                {
                    continue;
                }
                if (body.IsDestroyed)
                {
                    continue;
                }
                Vector2 direction = (body.Position - Position);
                float distance = direction.Magnitude();
                if (distance > Radius + body.Radius)
                {
                    float force = GravityForce * Mass * body.Mass / (distance * distance);

                    Vector2 directedForce = direction.Normalized() * force;
                    totalForce += directedForce;
                    bodyCount++;
                }
            }
            if (bodyCount > 0)
            {
                Vector2 averageForce = totalForce / bodyCount;
                Vector2 acceleration = averageForce / Mass;
                Velocity += acceleration * deltaTime;
            }
        }

        public void UpdatePosition(float deltaTime)
        {
            if (IsDestroyed)
            {
                return;
            }
            if (!IsStationary)
            {
                Position += Velocity * deltaTime;
            }
        }

        public SpaceBody(SpaceBody other)
        {
            Position = new Vector2(other.Position);
            Velocity = new Vector2(other.Velocity);
            Radius = other.Radius;
            IsStationary = other.IsStationary;
            IsDestroyed = other.IsDestroyed;
        }

        bool VelocityWillGoOffscreen(Rect bounds)
        {
            Vector2 nextPosition = Position + Velocity;
            return !bounds.ContainsPoint(nextPosition);
        }

        #region IDraggableObject Members

        public bool MouseIsOnObject(Vector2 mousePosition)
        {
            return DistanceFromMouse(mousePosition) <= Radius + 1.0f;
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
