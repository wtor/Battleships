using System;
using System.Collections.Generic;
using Battleships.Core.Common;
using Battleships.Core.Services;

namespace Battleships.Core.Ships
{
    public abstract class Destroyer : BaseShip
    {
        public override string Name => "Destroyer";
        public override char Symbol => 'D';

        protected static Size DefaultVerticalSize => new Size(1, 4);

        protected Destroyer(Position startPosition, Size size) : base(startPosition, size)
        {
        }
    }

    public sealed class VerticalDestroyer : Destroyer
    {
        public VerticalDestroyer(Position startPosition) : base(startPosition, DefaultVerticalSize)
        {
        }
    }

    public sealed class HorizontalDestroyer : Destroyer
    {
        public HorizontalDestroyer(Position startPosition) : base(startPosition, DefaultVerticalSize.Transpose())
        {
        }
    }

    public sealed class DestroyerFactory : BaseShipFactory
    {
        public DestroyerFactory(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator, GetShipFactories())
        {
        }

        private static IReadOnlyList<Func<Position, IShip>> GetShipFactories()
        {
            return new Func<Position, IShip>[]
            {
                position => new HorizontalDestroyer(position),
                position => new VerticalDestroyer(position),
            };
        }
    }
}