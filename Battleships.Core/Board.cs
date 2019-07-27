using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;
using Battleships.Core.Ships;

namespace Battleships.Core
{
    public interface IBoard
    {
        void Shot(Shot shot);

        event EventHandler<AccurateShotEventArgs> AccurateShot;

        event EventHandler<MissedShotEventArgs> MissedShot;

        event EventHandler<RepeatedShotEventArgs> RepeatedShot;

        event EventHandler<ShipSunkEventArgs> ShipSunk;

        event EventHandler<AllShipsSunkEventArgs> AllShipsSunk;

        /// <summary>
        /// Returns fields with state that is visible for a player.
        /// </summary>
        IReadOnlyList<IField> GetPlayerVisibleFields();
    }

    public sealed class Board : IBoard
    {
        private readonly IField[,] boardStateFields;
        private readonly IField[,] playerFields;

        private IReadOnlyList<IShip> Ships { get; }

        private readonly List<Shot> shotHistory;

        public event EventHandler<AccurateShotEventArgs> AccurateShot;

        public event EventHandler<MissedShotEventArgs> MissedShot;

        public event EventHandler<RepeatedShotEventArgs> RepeatedShot;

        public event EventHandler<ShipSunkEventArgs> ShipSunk;

        public event EventHandler<AllShipsSunkEventArgs> AllShipsSunk;

        public Board(Size size, IReadOnlyList<IShip> ships)
        {
            Ships = ships;
            shotHistory = new List<Shot>();
            boardStateFields = BuildBoardStateMatrix(size);
            playerFields = BuildPlayerMatrix(size);
        }

        public IReadOnlyList<IField> GetBoardStateFields()
        {
            return boardStateFields
                .Cast<IField>()
                .ToArray();
        }

        public IReadOnlyList<IField> GetPlayerVisibleFields()
        {
            return playerFields
                .Cast<IField>()
                .ToArray();
        }

        private IField[,] BuildBoardStateMatrix(Size size)
        {
            var fields = new IField[size.Height, size.Width];

            for (uint x = 0; x < size.Width; x++)
            for (uint y = 0; y < size.Height; y++)
            {
                fields[x, y] = new EmptyField(new Position(x, y));
            }

            foreach (var ship in Ships)
            foreach (var shipPosition in ship.Positions)
            {
                fields[shipPosition.X, shipPosition.Y] = new ShipField(shipPosition, ship);
            }

            return fields;
        }

        private IField[,] BuildPlayerMatrix(Size size)
        {
            var fields = new IField[size.Height, size.Width];

            for (uint x = 0; x < size.Width; x++)
            for (uint y = 0; y < size.Height; y++)
            {
                fields[x, y] = new UnknownField(new Position(x, y));
            }

            return fields;
        }
        
        public void Shot(Shot shot)
        {
            if (shotHistory.Contains(shot))
            {
                RepeatedShot?.Invoke(this, new RepeatedShotEventArgs(shot, new ReadOnlyBoardContext(this)));
                return;
            }

            shotHistory.Add(shot);

            var shipToShot = Ships.SingleOrDefault(s => s.CollidesWith(shot));

            if (shipToShot == null)
            {
                HandleMissedShot(shot);

                return;
            }

            shipToShot.GetShot(shot);

            HandleAccurateShot(shot, shipToShot);
        }

        private void HandleMissedShot(Shot shot)
        {
            SetField(boardStateFields, shot.Position, new MissedShotField(shot.Position));
            SetField(playerFields, shot.Position, new MissedShotField(shot.Position));

            MissedShot?.Invoke(this, new MissedShotEventArgs(new ReadOnlyBoardContext(this)));
        }

        private void HandleAccurateShot(Shot shot, IShip shotShip)
        {
            SetField(boardStateFields, shot.Position, new ShootShipField(shot.Position, shotShip));
            SetField(playerFields, shot.Position, new ShootShipField(shot.Position, shotShip));

            var boardContext = new ReadOnlyBoardContext(this);

            if (shotShip.IsSunk)
            {
                ShipSunk?.Invoke(this, new ShipSunkEventArgs(shotShip, boardContext));

                if (Ships.All(s => s.IsSunk))
                    AllShipsSunk?.Invoke(this, new AllShipsSunkEventArgs(shotHistory.Count, boardContext));
            }
            else
                AccurateShot?.Invoke(this, new AccurateShotEventArgs(shotShip.Name, boardContext));
        }

        private static void SetField(IField[,] fields, Position position, IField value)
        {
            fields[position.X, position.Y] = value;
        }
    }
}