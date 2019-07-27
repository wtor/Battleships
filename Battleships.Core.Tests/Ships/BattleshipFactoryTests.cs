using Battleships.Core.Common;
using Battleships.Core.Ships;
using Battleships.Core.Tests.TestInfrastructure;
using NUnit.Framework;

namespace Battleships.Core.Tests.Ships
{
    public sealed class BattleshipFactoryTests
    {
        [Test]
        public void Factory_creates_battleship_with_valid_starting_position()
        {
            var factory = new BattleshipFactory(new DeterministicModuloRandomNumberGenerator());

            var startingPosition = new Position(3, 3);
            var ship = factory.CreateAt(startingPosition);

            Assert.That(ship.StartPosition, Is.EqualTo(startingPosition));
        }

        [Test]
        public void Factory_creates_valid_vertical_battleship()
        {
            var randomNumberGenerator = new DeterministicModuloRandomNumberGenerator();
            randomNumberGenerator.Next(0, 1);

            var factory = new BattleshipFactory(randomNumberGenerator);

            var ship = factory.CreateAt(new Position(3, 3));

            var expectedPositions = new[]
            {
                new Position(3, 3),
                new Position(3, 4),
                new Position(3, 5),
                new Position(3, 6),
                new Position(3, 7),
            };

            Assert.That(ship.Positions, Has.Exactly(5).Items);
            Assert.That(ship.Positions, Is.EquivalentTo(expectedPositions));
        }

        [Test]
        public void Factory_creates_valid_horizontal_battleship()
        {
            var randomNumberGenerator = new DeterministicModuloRandomNumberGenerator();

            var factory = new BattleshipFactory(randomNumberGenerator);

            var ship = factory.CreateAt(new Position(3, 3));

            var expectedPositions = new[]
            {
                new Position(3, 3),
                new Position(4, 3),
                new Position(5, 3),
                new Position(6, 3),
                new Position(7, 3),
            };

            Assert.That(ship.Positions, Has.Exactly(5).Items);
            Assert.That(ship.Positions, Is.EquivalentTo(expectedPositions));
        }
    }
}