using System;
using System.Drawing;
using System.Xml.Serialization;

namespace PhysSim
{
    public struct Vector2
    {
        /// <summary>
        /// The component in the Right direction
        /// </summary>
        [XmlAttribute]
        public double X;
        /// <summary>
        /// The component in the Up direction
        /// </summary>
        [XmlAttribute]
        public double Y;

        /// <summary>
        /// Creates a cartesian Vector out of cartesian coordinates
        /// </summary>
        /// <param name="x">The component in the Right direction</param>
        /// <param name="y">The component in the Up direction</param>
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the sum of the squares of the X and Y components
        /// </summary>
        public double SqrMag
        {
            get
            {
                return X * X + Y * Y;
            }
        }
        /// <summary>
        /// Gets the length of the vector
        /// </summary>
        public double Magnitude
        {
            get
            {
                return Math.Sqrt(SqrMag);
            }
        }

        /// <summary>
        /// Gets a value to quickly disprove equality, but does not prove equality
        /// </summary>
        public override int GetHashCode()
        {
            return (int)X * (int)Y;
        }
        /// <summary>
        /// Returns false if obj is null. Returns true if obj is equal to this vector. Otherwise throws an exception if obj is not a Vector2
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Vector2)) return false;
            return (Vector2)obj == this;
        }

        /// <summary>
        /// Returns the dot product of two vectors
        /// </summary>
        public static double Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <summary>
        /// Gets the angle between two vectors. Values lie within the interval [0, pi]
        /// </summary>
        public static double Angle(Vector2 a, Vector2 b)
        {
            return Math.Acos(Dot(a, b) / (a.Magnitude * b.Magnitude));
        }

        /// <summary>
        /// Returns the angle between this vector and Right as used in the polar coordinate system. Values lie within the interval (-pi, pi]
        /// </summary>
        public double Theta
        {
            get
            {
                double res = Math.Atan2(Y, X);
                return res < 0d ? res + 2 * Math.PI : res;
            }
        }

        /// <summary>
        /// Creates a cartesian vector from polar coordinates
        /// </summary>
        /// <param name="theta">The angle between the resultant vector and Right</param>
        /// <param name="mag">The magnitude of the returned vector</param>
        public static Vector2 FromPolar(double theta, double mag)
        {
            return new Vector2(mag * Math.Cos(theta), mag * Math.Sin(theta));
        }

        /// <summary>
        /// Returns a copy of this vector with a magnitude of 1
        /// </summary>
        public Vector2 Unit
        {
            get
            {
                return this / Magnitude;
            }
        }

        /// <summary>
        /// The unit vector in the Up direction (0, 1)
        /// </summary>
        public static Vector2 Up = new Vector2(0d, 1d);
        /// <summary>
        /// The unit vector in the Down direction (0, -1)
        /// </summary>
        public static Vector2 Down = new Vector2(0d, -1d);
        /// <summary>
        /// The unit vector in the Right direction (1, 0)
        /// </summary>
        public static Vector2 Right = new Vector2(1d, 0d);
        /// <summary>
        /// The unit vector in the Left direction (-1, 0)
        /// </summary>
        public static Vector2 Left = new Vector2(-1d, 0d);

        /// <summary>
        /// The Zero vector (0, 0)
        /// </summary>
        public static Vector2 Zero = new Vector2(0, 0);

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 v, double a)
        {
            return new Vector2(v.X * a, v.Y * a);
        }
        public static Vector2 operator *(double a, Vector2 v)
        {
            return new Vector2(v.X * a, v.Y * a);
        }
        public static Vector2 operator /(Vector2 v, double a)
        {
            return new Vector2(v.X / a, v.Y / a);
        }

        public static implicit operator PointF(Vector2 v)
        {
            return new PointF((float)v.X, (float)v.Y);
        }
        public static implicit operator Point(Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }
        public static implicit operator Size(Vector2 v)
        {
            return new Size((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }

        public static implicit operator Vector2(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        public static implicit operator Vector2(Size s)
        {
            return new Vector2(s.Width, s.Height);
        }

        /// <summary>
        /// Returns the projection of vector a onto vector b
        /// </summary>
        /// <param name="a">The vector to be projected</param>
        /// <param name="b">The vector to be projected onto</param>
        public static Vector2 Project(Vector2 a, Vector2 b)
        {
            return b * (Dot(a, b) / b.SqrMag);
        }

        private static Random random = new Random();
        /// <summary>
        /// Returns a cartesian vector that points in a random direction and has a magnitude of 1
        /// </summary>
        public static Vector2 RandomUnit
        {
            get
            {
                return FromPolar(2 * Math.PI * random.NextDouble(), 1d);
            }
        }

        private const string FormatString = "({0}, {1})";
        /// <summary>
        /// Gets this vector's cartesian coordinates in point form (X, Y)
        /// </summary>
        public override string ToString()
        {
            return string.Format(FormatString, X, Y);
        }
    }
}
