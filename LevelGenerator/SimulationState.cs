using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelGenerator
{
    class SimulationState
    {
        public const int SimulationFramesPerSecond = 50;
        public const float FrameDeltaTime = 1.0f / SimulationFramesPerSecond;
        public List<SpaceBody> IndependentBodies { get; set; }
        public Circle GoalArea { get; set; }
        public List<Circle> AvoidAreas { get; set; }
        public SpaceBody ControlledBody { get; set; }

        public SimulationState(Level level, Vector2 mousePosition)
        {
            IndependentBodies = new List<SpaceBody>();
            foreach (var independentBody in level.IndependentBodies)
            {
                IndependentBodies.Add(new SpaceBody(independentBody));
            }
            ControlledBody = new SpaceBody(level.ControlledBody);
            GoalArea = level.GoalArea;
            AvoidAreas = new List<Circle>();
            foreach (var avoidArea in level.AvoidAreas)
            {
                AvoidAreas.Add(new Circle(avoidArea));
            }
            ControlledBody.Velocity = (mousePosition - ControlledBody.Position) / 2.0f;
        }

        private IEnumerable<SpaceBody> AllBodies()
        {
            foreach (var independentBody in IndependentBodies)
            {
                yield return independentBody;
            }
            yield return ControlledBody;
        }

        public bool EverythingVisible(Rect screenBounds)
        {
            if (!AvoidAreas.All(avoidArea => avoidArea.IsVisible(screenBounds)))
            {
                return false;
            }
            if (!GoalArea.IsVisible(screenBounds))
            {
                return false;
            }
            return true;
        }

        public bool BodiesWillCollide(SpaceBody pa, SpaceBody pb, float deltaTime)
        { 
            Vector2 s = pa.Position - pb.Position; // vector between the centers of each sphere
            Vector2 v = pa.Velocity - pb.Velocity; // relative velocity between spheres
            float r = pa.Radius + pb.Radius;
            float t;
 
            float c = s.Dot(s) - r*r; // if negative, they overlap
            if (c < 0.0f) // if true, they already overlap
            {
                t = 0.0f;
                return true;
            }
 
            float a = v.Dot(v);
            //if (a &lt; EPSILON)
            //  return false; // does not move towards each other
 
            float b = v.Dot(s);
            if (b <= 0.0f)
                return false; // does not move towards each other
 
            float d = b*b - a*c;
            if (d < 0.0f)
                return false; // no real roots ... no collision
 
            t = (-b - (float)Math.Sqrt(d)) / a;
 
            return t < deltaTime;
        }

        public bool MoveBodies()
        {
            bool fatalCollision = false;
            foreach (var body in AllBodies())
            {
                body.UpdateVelocity(AllBodies(), FrameDeltaTime);
            }
            foreach (var body in AllBodies())
            {
                if (body.IsDestroyed)
                {
                    continue;
                }
                foreach (var otherBody in AllBodies())
                {
                    if (otherBody.IsDestroyed)
                    {
                        continue;
                    }
                    if (body == otherBody)
                    {
                        continue;
                    }
                    if (BodiesWillCollide(body, otherBody, FrameDeltaTime))
                    {
                        SpaceBody greaterBody;
                        SpaceBody lesserBody;
                        if (body.Mass > otherBody.Mass ||
                            (body.Mass == otherBody.Mass && otherBody != ControlledBody))
                        {
                            greaterBody = body;
                            lesserBody = otherBody;
                        }
                        else
                        {
                            greaterBody = otherBody;
                            lesserBody = body;
                        }

                        if (lesserBody == ControlledBody)
                        {
                            fatalCollision = true;
                        }

                        float totalMass = greaterBody.Mass + lesserBody.Mass;
                        Vector2 averageVelocity = (greaterBody.Velocity * greaterBody.Mass + lesserBody.Velocity * lesserBody.Mass) / totalMass;
                        Vector2 averagePosition = (greaterBody.Position * greaterBody.Mass + lesserBody.Position * lesserBody.Mass) / totalMass;
                        greaterBody.Mass = totalMass;
                        lesserBody.Mass = 0.0f;
                        greaterBody.Velocity = averageVelocity;
                        lesserBody.Velocity = new Vector2(0, 0);
                        greaterBody.Position = averagePosition;
                        lesserBody.IsDestroyed = true;
                    }
                }
            }
            foreach (var body in AllBodies())
            {
                body.UpdatePosition(FrameDeltaTime);
            }
            return fatalCollision;
        }

        public void Draw(Rect screenBounds)
        {
            foreach (var independentBody in IndependentBodies)
            {
                independentBody.Draw(Color.FromArgb(128, independentBody.IsStationary ? Color.Purple : Color.Blue), true, screenBounds);
            }
            ControlledBody.Draw(Color.FromArgb(128, Color.SkyBlue), false, screenBounds);
        }

        public bool Simulate(bool checkAvoidArea, Rect screenBounds, int maxFrameCount, out bool hadOffscreen, out float totalControlledBodyTravelDistance, out Vector2 winPosition)
        {
            hadOffscreen = false;
            totalControlledBodyTravelDistance = 0.0f;
            winPosition = new Vector2(0,0);
            for (int frameI = 0; frameI < maxFrameCount; frameI++)
            {
                bool collision = MoveBodies();
                if (!ControlledBody.IsVisible(screenBounds))
                {
                    hadOffscreen = true;
                }
                if (!IndependentBodies[0].IsVisible(screenBounds))
                {
                    hadOffscreen = true;
                }
                totalControlledBodyTravelDistance += ControlledBody.Velocity.Magnitude() * FrameDeltaTime;
                if (ControlledBody.Velocity.Magnitude() * FrameDeltaTime > 30)
                {
                    return false;
                }

                if ((ControlledBody.Position - GoalArea.Position).Magnitude() < GoalArea.Radius - ControlledBody.Radius / 2.0f)
                {
                    winPosition = new Vector2(ControlledBody.Position);
                    return true;
                }

                if (checkAvoidArea)
                {
                    foreach (var avoidArea in AvoidAreas)
                    {
                        if ((ControlledBody.Position - avoidArea.Position).Magnitude() < avoidArea.Radius + ControlledBody.Radius / 2.0f)
                        {
                            return false;
                        }
                    }
                }

                if (ControlledBody.Position.Magnitude() > screenBounds.Width)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
