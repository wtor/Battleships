using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;
using Battleships.Core.Ships;

namespace Battleships.Core.Services
{
    public interface IShipGenerationService
    {
        IReadOnlyList<IShip> GenerateShips(IShipFactory shipFactory, int count, IReadOnlyList<IShip> existingShips);
    }

    public sealed class ShipGenerationService : IShipGenerationService
    {
        private readonly Size boardSize;
        private readonly IRandomPositionGenerator randomPositionGenerator;

        public ShipGenerationService(Size boardSize, IRandomPositionGenerator randomPositionGenerator)
        {
            this.boardSize = boardSize;
            this.randomPositionGenerator = randomPositionGenerator;
        }

        public IReadOnlyList<IShip> GenerateShips(IShipFactory shipFactory, int count, IReadOnlyList<IShip> existingShips)
        {
            return GenerateShipsInternal()
                .Take(count)
                .ToList();

            IEnumerable<IShip> GenerateShipsInternal()
            {
                var generatedShips = new List<IShip>();

                while (true)
                {
                    var randomPosition = randomPositionGenerator.GetNext(boardSize);
                    var ship = shipFactory.CreateAt(randomPosition);

                    if (ship.Positions.Any(p => !p.IsIncludedIn(boardSize)))
                        continue;

                    if (existingShips.Concat(generatedShips).Any(s => s.CollidesWith(ship)))
                        continue;

                    generatedShips.Add(ship);

                    yield return ship;
                }
            }
        }
    }
}