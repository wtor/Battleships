using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;

namespace Battleships.Core
{
    public sealed class Shot : ICollideable, IEquatable<Shot>
    {
        public Position Position { get; }

        public Shot(Position position)
        {
            Position = position;
        }

        IEnumerable<Position> ICollideable.Positions => new[] {Position};

        public bool CollidesWith(ICollideable other)
        {
            return other.Positions.Any(p => p == Position);
        }

        public bool Equals(Shot other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Position, other.Position);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Shot other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Position != null ? Position.GetHashCode() : 0);
        }

        public static bool operator ==(Shot left, Shot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Shot left, Shot right)
        {
            return !Equals(left, right);
        }
    }
}