using Battleships.Core.Common;

namespace Battleships.Core.Ships
{
    public interface IShip : ICollideable
    {
        /// <summary>
        /// Position of left upper corner of the ship.
        /// </summary>
        Position StartPosition { get; }

        string Name { get; }

        char Symbol { get; }

        bool IsSunk { get; }

        void GetShot(Shot shot);
    }
}