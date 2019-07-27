using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;
using Battleships.Core.Ships;
using NUnit.Framework;

namespace Battleships.Core.Tests
{
    public sealed class BoardTests
    {
        private Size boardSize;

        private IShip battleship;
        private IShip destroyer;

        private IShip[] ships;

        [SetUp]
        public void SetUp()
        {
            boardSize = new Size(10, 10);

            battleship = new VerticalBattleship(new Position(0, 1));
            destroyer = new HorizontalDestroyer(new Position(2, 2));

            ships = new[]
            {
                battleship,
                destroyer
            };
        }

        [Test]
        public void All_ships_has_valid_positions()
        {
            var board = new Board(boardSize, ships);

            var knownFields = board.GetBoardStateFields();

            var battleshipFieldsPositions = knownFields
                .GetPositionsByShipName(battleship.Name);

            var destroyerFieldsPositions = knownFields
                .GetPositionsByShipName(destroyer.Name);

            Assert.That(battleshipFieldsPositions, Has.Exactly(5).Items);
            Assert.That(destroyerFieldsPositions, Has.Exactly(4).Items);

            var expectedBattleshipPositions = new[]
            {
                new Position(0, 1),
                new Position(0, 2),
                new Position(0, 3),
                new Position(0, 4),
                new Position(0, 5),
            };

            var expectedDestroyerPositions = new[]
            {
                new Position(2, 2),
                new Position(3, 2),
                new Position(4, 2),
                new Position(5, 2),
            };

            Assert.That(battleshipFieldsPositions, Is.EquivalentTo(expectedBattleshipPositions));
            Assert.That(destroyerFieldsPositions, Is.EquivalentTo(expectedDestroyerPositions));
        }

        [Test]
        public void When_game_begins_player_do_not_know_any_ship_position()
        {
            var board = new Board(boardSize, ships);

            Assert.That(board.GetPlayerVisibleFields(), Has.All.TypeOf<UnknownField>());
        }

        [Test]
        public void When_shot_misses_ship_fields_are_update()
        {
            var board = new Board(boardSize, ships);

            var shotPosition = new Position(9, 9);
            board.Shot(new Shot(shotPosition));

            var missedUnknownField = board
                .GetPlayerVisibleFields()
                .GetFieldByPosition(shotPosition);

            var missedKnownField = board
                .GetBoardStateFields()
                .GetFieldByPosition(shotPosition);

            Assert.That(missedUnknownField, Is.TypeOf<MissedShotField>());
            Assert.That(missedKnownField, Is.TypeOf<MissedShotField>());
        }

        [Test]
        public void When_shot_is_repeated_nothing_happens()
        {
            var board = new Board(boardSize, ships);

            var shotPosition = new Position(9, 9);
            board.Shot(new Shot(shotPosition));
            board.Shot(new Shot(shotPosition));

            var missedUnknownField = board
                .GetPlayerVisibleFields()
                .GetFieldByPosition(shotPosition);

            var missedKnownField = board
                .GetBoardStateFields()
                .GetFieldByPosition(shotPosition);

            Assert.That(missedUnknownField, Is.TypeOf<MissedShotField>());
            Assert.That(missedKnownField, Is.TypeOf<MissedShotField>());
        }

        [Test]
        public void When_shot_is_accurate_fields_are_updated()
        {
            var board = new Board(boardSize, ships);

            var shotPosition = new Position(0, 1);
            board.Shot(new Shot(shotPosition));

            var missedUnknownField = board
                .GetPlayerVisibleFields()
                .GetFieldByPosition(shotPosition);

            var missedKnownField = board
                .GetBoardStateFields()
                .GetFieldByPosition(shotPosition);

            Assert.That(missedUnknownField, Is.TypeOf<ShootShipField>());
            Assert.That(missedKnownField, Is.TypeOf<ShootShipField>());
        }

        [Test]
        public void When_shot_misses_MissedShot_event_gets_fired()
        {
            var eventFiresCount = 0;

            var board = new Board(boardSize, ships);
            board.MissedShot += (o, e) => eventFiresCount++;

            board.Shot(new Shot(new Position(9, 9)));

            Assert.That(eventFiresCount, Is.EqualTo(1));
        }

        [Test]
        public void When_shot_is_accurate_AccurateShot_event_gets_fired()
        {
            var eventFiresCount = 0;

            var board = new Board(boardSize, ships);
            board.AccurateShot += (o, e) => eventFiresCount++;

            board.Shot(new Shot(new Position(0, 1)));

            Assert.That(eventFiresCount, Is.EqualTo(1));
        }

        [Test]
        public void When_shot_is_repeated_RepeatedShot_event_gets_fired()
        {
            var eventFiresCount = 0;

            var board = new Board(boardSize, ships);
            board.RepeatedShot += (o, e) => eventFiresCount++;

            board.Shot(new Shot(new Position(0, 1)));
            board.Shot(new Shot(new Position(0, 1)));

            Assert.That(eventFiresCount, Is.EqualTo(1));
        }

        [Test]
        public void When_ship_sinks_ShipSunk_event_gets_fired()
        {
            var eventFiresCount = 0;

            var board = new Board(boardSize, ships);
            board.ShipSunk += (o, e) => eventFiresCount++;

            foreach (var position in destroyer.Positions)
                board.Shot(new Shot(position));

            Assert.That(eventFiresCount, Is.EqualTo(1));
        }

        [Test]
        public void When_all_ships_are_sunk_AllShipsSunk_event_gets_fired()
        {
            var eventFiresCount = 0;

            var board = new Board(boardSize, ships);
            board.AllShipsSunk += (o, e) => eventFiresCount++;

            foreach (var ship in ships)
            foreach (var position in ship.Positions)
                board.Shot(new Shot(position));

            Assert.That(eventFiresCount, Is.EqualTo(1));
        }
    }

    public static class FieldListExtensions
    {
        public static IField GetFieldByPosition(this IReadOnlyList<IField> fields, Position position)
        {
            return fields.Single(f => f.Position == position);
        }
        public static IReadOnlyList<Position> GetPositionsByShipName(this IReadOnlyList<IField> fields, string shipName)
        {
            return fields
                .OfType<ShipField>()
                .Where(s => s.ShipName == shipName)
                .OrderBy(s => s.Position)
                .Select(s => s.Position)
                .ToList();
        }

    }
}