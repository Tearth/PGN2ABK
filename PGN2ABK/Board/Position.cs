using System;
using System.Security.Cryptography.X509Certificates;

namespace PGN2ABK.Board
{
    public struct Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static Position Zero = new Position(0, 0);

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Abs()
        {
            return new Position(Math.Abs(X), Math.Abs(Y));
        }

        public bool IsValid()
        {
            return X >= 1 && X <= 8 && Y >= 1 && Y <= 8;
        }

        public static Position operator+(Position a, Position b)
        {
            return new Position(a.X + b.X, a.Y + b.Y);
        }

        public static Position operator-(Position a, Position b)
        {
            return new Position(a.X - b.X, a.Y - b.Y);
        }

        public static bool operator ==(Position a, Position b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
