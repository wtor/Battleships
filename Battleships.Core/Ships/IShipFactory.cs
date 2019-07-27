using System;
using System.Collections.Generic;
using Battleships.Core.Common;
using Battleships.Core.Services;

namespace Battleships.Core.Ships
{
    public interface IShipFactory
    {
        IShip CreateAt(Position position);
    }

    public abstract class BaseShipFactory : IShipFactory
    {
        private readonly IRandomNumberGenerator randomNumberGenerator;

        private readonly IReadOnlyList<Func<Position, IShip>> shipFactories;

        protected BaseShipFactory(IRandomNumberGenerator randomNumberGenerator, IReadOnlyList<Func<Position, IShip>> shipFactories)
        {
            this.randomNumberGenerator = randomNumberGenerator;
            this.shipFactories = shipFactories;
        }

        public IShip CreateAt(Position position)
        {
            var randomFactoryIndex = randomNumberGenerator.Next(0, shipFactories.Count - 1);

            return shipFactories[randomFactoryIndex](position);
        }
    }
}