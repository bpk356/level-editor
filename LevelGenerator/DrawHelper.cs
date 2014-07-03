using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace LevelGenerator
{
    class DrawHelper
    {
        public static void DrawCircle(Vector2 center, float radius, Color color, Rect screenBounds)
        {
            Vector2 scaledCenter = new Vector2(center);
            scaledCenter.X -= screenBounds.MinX;
            scaledCenter.Y -= screenBounds.MinY;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.Blend);
            GL.LoadIdentity();
            GL.Color4(color);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(PrimitiveType.TriangleFan);
            for (int i = 0; i < 360; i += 10)
            {
                double degInRad = i * 3.1416 / 180;
                GL.Vertex2(Math.Cos(degInRad) * radius + scaledCenter.X, Math.Sin(degInRad) * radius + scaledCenter.Y);
            }
            GL.End();

            //g.DrawEllipse(new Pen(new SolidBrush(color), 1), scaledCenter.X - radius, scaledCenter.Y - radius, 2 * radius, 2 * radius);
        }

        public static void DrawVelocity(Vector2 position, Vector2 velocity, Color color, Rect screenBounds)
        {
            Vector2 scaledPosition = new Vector2(position);
            scaledPosition.X -= screenBounds.MinX;
            scaledPosition.Y -= screenBounds.MinY;

            Vector2 scaledVelocity = scaledPosition + velocity;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color3(color);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(scaledPosition.X, scaledPosition.Y);
            GL.Vertex2(scaledVelocity.X, scaledVelocity.Y);
            GL.End();
        }

        public static void SetPixel(Vector2 position, Color color, Rect screenBounds, int size)
        {
            Vector2 scaledPosition = new Vector2(position);
            scaledPosition.X -= screenBounds.MinX;
            scaledPosition.Y -= screenBounds.MinY;

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(color);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(scaledPosition.X, scaledPosition.Y);
            GL.Vertex2(scaledPosition.X, scaledPosition.Y + size);
            GL.Vertex2(scaledPosition.X + size, scaledPosition.Y + size);
            GL.Vertex2(scaledPosition.X + size, scaledPosition.Y);
            GL.End();
        }
    }
}
