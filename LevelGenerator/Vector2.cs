using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelGenerator
{
    class Vector2 : IEquatable<Vector2>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public const int MutatableAttributeCount = 4;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(Vector2 other)
        {
            X = other.X;
            Y = other.Y;
        }

        public void MutateAttribute(int targetAttribute)
        {
            if (targetAttribute == 0)
            {
                X += 1.0f;
            }
            else if (targetAttribute == 1)
            {
                X -= 1.0f;
            }
            else if (targetAttribute == 2)
            {
                Y += 1.0f;
            }
            else
            {
                Y -= 1.0f;
            }
            
        }

        public override string ToString()
        {
            return X + "," + Y;
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2 operator *(Vector2 left, float right)
        {
            return new Vector2(left.X * right, left.Y * right);
        }

        public static Vector2 operator /(Vector2 left, float right)
        {
            return new Vector2(left.X / right, left.Y / right);
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public Vector2 Normalized()
        {
            float magnitude = Magnitude();
            return new Vector2(X / magnitude, Y / magnitude);
        }

        public static readonly Vector2 Left = new Vector2(-1.0f, 0.0f);
        public static readonly Vector2 Right = new Vector2(1.0f, 0.0f);
        public static readonly Vector2 Up = new Vector2(0.0f, 1.0f);
        public static readonly Vector2 Down = new Vector2(0.0f, -1.0f);

        #region IEquatable<Vector2> Members

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        #endregion

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
