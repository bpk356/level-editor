using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LevelGenerator
{
    class SpaceBody
    {
        public const float GravityForce = 40000.0f;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int Radius { get; set; }
        public float Mass { get { return (float)Math.Pow(10, Radius); } }

        public const int MutatableAttributeCount = Vector2.MutatableAttributeCount * 2 + 2;

        public SpaceBody(Random generator, Rect bounds)
        {
            Position = new Vector2((float)generator.NextDouble() * bounds.Width + bounds.MinX, (float)generator.NextDouble() * bounds.Height + bounds.MinY);
            Radius = generator.Next(3) + 2;
            do
            {
                Velocity = new Vector2((float)generator.NextDouble() * bounds.Width + bounds.MinX, (float)generator.NextDouble() * bounds.Height + bounds.MinY);
            } while (VelocityWillGoOffscreen(bounds));
            Velocity = new Vector2(0, 0);
        }

        public SpaceBody(Vector2 position, Vector2 velocity, int radius)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
        }

        public void Draw(Color color, bool drawVelocity, Rect screenBounds)
        {
            DrawHelper.DrawCircle(Position, Radius, color, screenBounds);
            if (drawVelocity)
            {
                DrawHelper.DrawVelocity(Position, Velocity, color, screenBounds);
            }
        }

        public override string ToString()
        {
            return "Position: " + Position.ToString() + ", Velocity: " + Velocity.ToString() + ", Mass: " + Mass;
        }

        public bool IsVisible(Rect bounds)
        {
            return bounds.ContainsPoint(Position);
        }

        public bool SimulateMotion(IEnumerable<SpaceBody> allBodies, float deltaTime)
        {
            Vector2 totalForce = new Vector2(0, 0);
            int bodyCount = 0;

            foreach (var body in allBodies)
            {
                if (body == this)
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
                else
                {
                    return true;
                }
            }
            if (bodyCount > 0)
            {
                Vector2 averageForce = totalForce / bodyCount;
                Vector2 acceleration = averageForce / Mass;
                Velocity += acceleration * deltaTime;
            }
            Position += Velocity * deltaTime;
            return false;
        }

        public SpaceBody(SpaceBody other)
        {
            Position = new Vector2(other.Position);
            Velocity = new Vector2(other.Velocity);
            Radius = other.Radius;
        }

        bool VelocityWillGoOffscreen(Rect bounds)
        {
            Vector2 nextPosition = Position + Velocity;
            return !bounds.ContainsPoint(nextPosition);
        }
    }
}
