using System;

namespace BlyadTheftAuto.Structs
{
    internal struct Vector3D
    {
        public float X;

        public float Y;

        public float Z;

		public Vector3D(Vector2D vec2, float z)
		{
			X = vec2.X;
			Y = vec2.Y;
			Z = z;
		}

        public Vector3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3D Zero
        {
            get
            {
                return new Vector3D();
            }
        }

        public float Length()
        {
            return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
        }

        public void Rotate()
        {
            var ret = new Vector3D()
            {
                X = this.Z,
                Y = this.Y,
                Z = this.X
            };
            this = ret;
        }

        public float LengthSqr()
        {
            return (X * X + Y * Y + Z * Z);
        }

		public bool IsEmpty
		{
			get
			{
				return this == Zero;
			}
		}

		public static float Distance(Vector3D a, Vector3D b)
        {
            var vec3 = a - b;
            var single = (float)Math.Sqrt((double)(vec3.X * vec3.X + vec3.Y * vec3.Y + vec3.Z * vec3.Z));
            return single;
        }

        public float DistanceInMetres(Vector3D other)
        {
            return Distance(this, other) * 0.01905f;
        }

        public float DistanceFrom(Vector3D vec)
        {
            return Distance(this, vec);
        }

        public static float Dot(Vector3D left, Vector3D right)
        {
            float single = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
            return single;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            var vec3 = new Vector3D
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
                Z = a.Z + b.Z
            };
            return vec3;
        }

        public static Vector3D operator *(Vector3D a, float b)
        {
            var vec3 = new Vector3D
            {
                X = a.X * b,
                Y = a.Y * b,
                Z = a.Z * b
            };
            return vec3;
        }

        public static Vector3D operator +(Vector3D a, float b)
        {
            var vec3 = new Vector3D
            {
                X = a.X + b,
                Y = a.Y + b,
                Z = a.Z + b
            };
            return vec3;
        }

        public static Vector3D operator -(Vector3D a, float b)
        {
            var vec3 = new Vector3D
            {
                X = a.X - b,
                Y = a.Y - b,
                Z = a.Z - b
            };
            return vec3;
        }

        public static Vector3D operator /(Vector3D value, float scale)
        {
            return new Vector3D(value.X / scale, value.Y / scale, value.Z / scale);
        }

        public static Vector3D operator *(Vector3D a, Vector3D b)
        {
            var vec3 = new Vector3D
            {
                X = a.Y * b.Z - a.Z * b.Y,
                Y = a.Z * b.X - a.X * b.Z,
                Z = a.X * b.Y - a.Y * b.X
            };
            return vec3;
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            var vec3 = new Vector3D
            {
                X = a.X - b.X,
                Y = a.Y - b.Y,
                Z = a.Z - b.Z
            };
            return vec3;
        }

        public override bool Equals(object other)
        {
            return this == (Vector3D)other;
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

		public override string ToString()
		{
			return $"X: {X}, Y: {Y}, Z: {Z}";
		}
    }
}