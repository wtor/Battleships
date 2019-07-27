using System;
using System.Collections.Generic;
using Battleships.Core.Common;
using Battleships.Core.Services;

namespace Battleships.Core.Ships
{
    public abstract class Battleship : BaseShip
    {
        protected static Size DefaultVerticalSize => new Size(1, 5);

        public override string Name => "Battleship";
        public override char Symbol => 'B';

        protected Battleship(Position startPosition, Size size) : base(startPosition, size)
        {
        }
    }

    public sealed class VerticalBattleship : Battleship
    {
        public VerticalBattleship(Position startPosition) : base(startPosition, DefaultVerticalSize)
        {
        }
    }

    public sealed class HorizontalBattleship : Battleship
    {
        public HorizontalBattleship(Position startPosition) : base(startPosition, DefaultVerticalSize.Transpose())
        {
        }
    }

    public sealed class BattleshipFactory : BaseShipFactory
    {
        public BattleshipFactory(IRandomNumberGenerator randomNumberGenerator)
            : base(randomNumberGenerator, GetShipFactories())
        {
        }

        private static IReadOnlyList<Func<Position, IShip>> GetShipFactories()
        {
            return new Func<Position, IShip>[]
            {
                position => new HorizontalBattleship(position),
                position => new VerticalBattleship(position),
            };
        }
    }
}