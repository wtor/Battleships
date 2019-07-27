using System;
using Battleships.Core.Common;
using Battleships.Core.Services;
using Battleships.Core.Ships;
using Battleships.Core.Tests.TestInfrastructure;
using NUnit.Framework;

namespace Battleships.Core.Tests.Services
{
    public sealed class ShipGenerationServiceTests
    {
        private Size boardSize;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            boardSize = new Size(8, 8);
        }

        [Test]
        public void Generates_valid_ship_when_there_are_no_ships()
        {
            var service = new ShipGenerationService(boardSize, GetRandomPositionGenerator(1, 2));

            var destroyerShipFactory = new DestroyerFactory(new DeterministicModuloRandomNumberGenerator());

            var ships = service.GenerateShips(destroyerShipFactory, 1, Array.Empty<IShip>());

            Assert.That(ships, Has.One.Items);

            var ship = ships[0];

            Assert.That(ship.StartPosition, Is.EqualTo(new Position(1, 2)));
        }

        [Test]
        public void Generates_valid_ships_when_first_overlaps_with_second()
        {
            // Starting point of both ships is (1, 2) so they must overlaps.
            var service = new ShipGenerationService(boardSize, GetRandomPositionGenerator(1, 2, 1, 2, 4, 4));

            var destroyerShipFactory = new DestroyerFactory(new DeterministicModuloRandomNumberGenerator());

            var ships = service.GenerateShips(destroyerShipFactory, 2, Array.Empty<IShip>());

            Assert.That(ships, Has.Exactly(2).Items);

            Assert.That(ships[0].StartPosition, Is.EqualTo(new Position(1, 2)));
            Assert.That(ships[1].StartPosition, Is.EqualTo(new Position(4, 4)));
        }

        [Test]
        public void Generates_valid_ship_when_ship_already_exists()
        {
            // Starting point of both ships is (1, 1) so they must overlaps.
            var service = new ShipGenerationService(boardSize, GetRandomPositionGenerator(1, 1, 4, 4));

            var destroyerShipFactory = new DestroyerFactory(new DeterministicModuloRandomNumberGenerator());

            var existingShip = new HorizontalBattleship(new Position(1, 1));
            var ships = service.GenerateShips(destroyerShipFactory, 1, new[] {existingShip});

            Assert.That(ships, Has.One.Items);

            Assert.That(ships[0].StartPosition, Is.EqualTo(new Position(4, 4)));
        }

        [Test]
        public void Generates_valid_ship_when_ship_position_exceeds_board_size()
        {
            // Ship at (7, 7) won't be created. Only (3, 3) will be created.
            var service = new ShipGenerationService(boardSize, GetRandomPositionGenerator(7, 7, 3, 3));

            var destroyerShipFactory = new DestroyerFactory(new DeterministicModuloRandomNumberGenerator());

            var existingShip = new HorizontalBattleship(new Position(1, 1));
            var ships = service.GenerateShips(destroyerShipFactory, 1, new[] {existingShip});

            Assert.That(ships, Has.One.Items);

            Assert.That(ships[0].StartPosition, Is.EqualTo(new Position(3, 3)));
        }

        private static RandomPositionGenerator GetRandomPositionGenerator(params int[] sequence)
        {
            return new RandomPositionGenerator(new DeterministicSequenceNumberGenerator(sequence));
        }
    }
}