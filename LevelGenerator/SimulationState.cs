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
        public const int SimulationFramesPerSecond = 30;
        public const float FrameDeltaTime = 1.0f / SimulationFramesPerSecond;
        public List<SpaceBody> IndependentBodies { get; set; }
        public Circle GoalArea { get; set; }
        public Circle AvoidArea { get; set; }
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
            AvoidArea = level.AvoidArea;
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
            if (!AvoidArea.IsVisible(screenBounds))
            {
                return false;
            }
            if (!GoalArea.IsVisible(screenBounds))
            {
                return false;
            }
            return true;
        }

        public bool MoveBodies()
        {
            bool collision = false;
            foreach (var body in AllBodies())
            {
                if (body.SimulateMotion(AllBodies(), FrameDeltaTime))
                {
                    collision = true;
                }
            }
            return collision;
        }

        public void Draw(Rect screenBounds)
        {
            foreach (var independentBody in IndependentBodies)
            {
                independentBody.Draw(Color.FromArgb(50, Color.Blue), true, screenBounds);
            }
            ControlledBody.Draw(Color.FromArgb(50, Color.SkyBlue), false, screenBounds);
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
                if (ControlledBody.Velocity.Magnitude() > screenBounds.Height / 2)
                {
                    return false;
                }

                if ((ControlledBody.Position - GoalArea.CenterPoint).Magnitude() < GoalArea.Radius - ControlledBody.Radius / 2.0f)
                {
                    winPosition = new Vector2(ControlledBody.Position);
                    return true;
                }

                if (checkAvoidArea && (ControlledBody.Position - AvoidArea.CenterPoint).Magnitude() < AvoidArea.Radius + ControlledBody.Radius / 2.0f)
                {
                    return false;
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
