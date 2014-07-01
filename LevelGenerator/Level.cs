using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LevelGenerator
{
    class Level
    {
        public List<SpaceBody> IndependentBodies { get; set; }
        public Circle GoalArea { get; set; }
        public Circle AvoidArea { get; set; }
        public SpaceBody ControlledBody { get; set; }

        public int MutatableAttributeCount
        {
            get
            {
                return SpaceBody.MutatableAttributeCount * IndependentBodies.Count + Circle.MutatableAttributeCount * 2 + Vector2.MutatableAttributeCount + 2;
            }
        }

        private Level()
        {
        }

        public Level(int independentBodyCount, Random generator, Rect bounds)
        {
            IndependentBodies = new List<SpaceBody>();
            for (int i = 0; i < independentBodyCount; i++)
            {
                IndependentBodies.Add(new SpaceBody(generator, bounds));
            }
            ControlledBody = new SpaceBody(generator, bounds);

            AvoidArea = GenerateRandomArea(generator, bounds);
            GoalArea = GenerateRandomArea(generator, bounds);            
        }

        public void Draw(Rect screenBounds)
        {
            GoalArea.Draw(Color.FromArgb(50, Color.Green), screenBounds);
            AvoidArea.Draw(Color.FromArgb(50, Color.Red), screenBounds);
            foreach (var independentBody in IndependentBodies)
            {
                independentBody.Draw(Color.FromArgb(50, Color.Blue), true, screenBounds);
            }
            ControlledBody.Draw(Color.FromArgb(50, Color.SkyBlue), false, screenBounds);
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

        public void WriteData()
        {
            Console.WriteLine("Controlled Body: " + ControlledBody.ToString());
            Console.WriteLine("Goal Area: " + GoalArea.ToString());
            Console.WriteLine("Avoid Area: " + AvoidArea.ToString());
            foreach (var independentBody in IndependentBodies)
            {
                Console.WriteLine("Independent body: " + independentBody.ToString());
            }
        }

        private Circle GenerateRandomArea(Random generator, Rect bounds)
        {
            float radius;
            Vector2 center;
            Vector2 left;
            Vector2 right;
            Vector2 up;
            Vector2 down;

            do
            {
                center = new Vector2((int)(generator.NextDouble() * bounds.Width + bounds.MinX), (int)(generator.NextDouble() * bounds.Height + bounds.MinY));
                radius = (int)(generator.NextDouble() * bounds.Height / 8.0 + bounds.Height / 16.0);

                left = center + Vector2.Left * radius;
                right = center + Vector2.Right * radius;
                up = center + Vector2.Up * radius;
                down = center + Vector2.Down * radius;
            } while (!bounds.ContainsPoint(left) || !bounds.ContainsPoint(right) || !bounds.ContainsPoint(up) || !bounds.ContainsPoint(down));

            return new Circle(center, radius);
        }
    }
}
