using System;
using System.Diagnostics;

namespace Battleships.Core.Common
{
    [DebuggerDisplay("({X}, {Y})")]
    public sealed class Position : IEquatable<Position>, IComparable<Position>
    {
        public uint X { get; }
        public uint Y { get; }

        public Position(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public bool IsIncludedIn(Size size)
        {
            return X <= size.Width - 1 && Y <= size.Height - 1;
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) X * 397) ^ (int) Y;
            }
        }

        public static bool operator ==(Position left, Position right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !Equals(left, right);
        }

        public int CompareTo(Position other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var xComparison = X.CompareTo(other.X);
            if (xComparison != 0)
                return xComparison;
            return Y.CompareTo(other.Y);
        }
    }
}