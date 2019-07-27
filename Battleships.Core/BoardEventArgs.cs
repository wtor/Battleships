using System;
using Battleships.Core.Ships;

namespace Battleships.Core
{
    public sealed class ShipSunkEventArgs : EventArgs
    {
        public ReadOnlyBoardContext Board { get; }
        public string ShipName { get; }

        public ShipSunkEventArgs(IShip ship, ReadOnlyBoardContext board)
        {
            Board = board;
            ShipName = ship.Name;
        }
    }

    public sealed class AccurateShotEventArgs : EventArgs
    {
        public string ShipName { get; }
        public ReadOnlyBoardContext Board { get; }

        public AccurateShotEventArgs(string shipName, ReadOnlyBoardContext board)
        {
            ShipName = shipName;
            Board = board;
        }
    }

    public sealed class MissedShotEventArgs : EventArgs
    {
        public ReadOnlyBoardContext Board { get; }

        public MissedShotEventArgs(ReadOnlyBoardContext board)
        {
            Board = board;
        }
    }

    public sealed class RepeatedShotEventArgs : EventArgs
    {
        public Shot Shot { get; }
        public ReadOnlyBoardContext Board { get; }

        public RepeatedShotEventArgs(Shot shot, ReadOnlyBoardContext board)
        {
            Shot = shot;
            Board = board;
        }
    }

    public sealed class AllShipsSunkEventArgs : EventArgs
    {
        public int ShotsCount { get; }
        public ReadOnlyBoardContext Board { get; }

        public AllShipsSunkEventArgs(int shotsCount, ReadOnlyBoardContext board)
        {
            ShotsCount = shotsCount;
            Board = board;
        }
    }
}