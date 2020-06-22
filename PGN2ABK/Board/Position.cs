using System;

namespace PGN2ABK.Board
{
    public readonly struct Position
    {
        public sbyte X { get; }
        public sbyte Y { get; }
        public static Position Zero = new Position(0, 0);

        public Position(sbyte x, sbyte y)
        {
            X = x;
            Y = y;
        }

        public Position(int x, int y) : this((sbyte)x, (sbyte)y)
        {

        }

        public Position Abs()
        {
            return new Position(Math.Abs(X), Math.Abs(Y));
        }

        public byte ToIndex()
        {
            return (byte)((X + Y * 8) - 1);
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

        public override bool Equals(object obj)
        {
            var position = obj as Position?;
            if (position == null)
            {
                return false;
            }

            return this == position;
        }

        public override int GetHashCode()
        {
            return X + Y * 8;
        }

        public override string ToString()
        {
            return $"[{X}, {Y}]";
        }
    }
}
