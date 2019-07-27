using System.Collections.Generic;

namespace Battleships.Core.Common
{
    public interface ICollideable
    {
        IEnumerable<Position> Positions { get; }

        bool CollidesWith(ICollideable other);
    }
}