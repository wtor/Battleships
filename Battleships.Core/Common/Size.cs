using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Battleships.Core.Common
{
    [DebuggerDisplay("{Width}x{Height}")]
    public sealed class Size : IEquatable<Size>
    {
        public uint Width { get; }
        public uint Height { get; }

        public Size(uint width, uint height)
        {
            Width = width;
            Height = height;
        }

        public Size Transpose()
        {
            return new Size(Height, Width);
        }

        public IEnumerable<Position> GetPositionsStartingFrom(Position startPosition)
        {
            for (uint x = 0; x < Width; x++)
            for (uint y = 0; y < Height; y++)
                yield return new Position(startPosition.X + x, startPosition.Y + y);
        }

        public bool Equals(Size other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Size other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Width * 397) ^ (int) Height;
            }
        }

        public static bool operator ==(Size left, Size right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Size left, Size right)
        {
            return !Equals(left, right);
        }
    }
}