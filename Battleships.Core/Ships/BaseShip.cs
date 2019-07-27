using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;

namespace Battleships.Core.Ships
{
    public abstract class BaseShip : IShip
    {
        public abstract string Name { get; }
        public abstract char Symbol { get; }

        public bool IsSunk => parts.All(p => p.IsShot);

        public void GetShot(Shot shot)
        {
            var collidingPart = parts
                .SingleOrDefault(p => p.Position == shot.Position);

            collidingPart?.Shot();
        }

        public Position StartPosition { get; }

        private readonly IList<ShipPart> parts;
        
        protected BaseShip(Position startPosition, Size size)
        {
            StartPosition = startPosition;
            parts = CreateParts(size);
        }

        private IList<ShipPart> CreateParts(Size size)
        {
            return size.GetPositionsStartingFrom(StartPosition)
                .Select(position => new ShipPart(position))
                .ToList();
        }

        public IEnumerable<Position> Positions
        {
            get { return parts.Select(p => p.Position); }
        }

        public bool CollidesWith(ICollideable other)
        {
            return Positions.Any(p => other.Positions.Contains(p));
        }

        private sealed class ShipPart
        {
            public Position Position { get; }

            public bool IsShot { get; private set; }

            public ShipPart(Position position)
            {
                Position = position;
            }

            public void Shot()
            {
                IsShot = true;
            }
        }
    }
}