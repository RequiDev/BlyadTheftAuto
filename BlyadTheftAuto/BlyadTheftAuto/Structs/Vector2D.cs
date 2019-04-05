using System;

namespace BlyadTheftAuto.Structs
{
    internal struct Vector2D
    {
        public float X;

        public float Y;

        public Vector2D(float x = 0.0f, float y = 0.0f)
        {
            X = x;
            Y = y;
        }

		public bool IsEmpty
		{
			get
			{
				return X == 0.0f && Y == 0.0f;
			}
		}

		public float DistanceFrom(Vector2D pointB)
        {
            double d1 = X - pointB.X;
            double d2 = Y - pointB.Y;

            return (float)Math.Sqrt(d1 * d1 + d2 * d2);
        }
    }
}
