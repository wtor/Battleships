using Battleships.Core.Common;

namespace Battleships.Core.Services
{
    public interface IRandomPositionGenerator
    {
        Position GetNext(Size validArea);
    }

    public sealed class RandomPositionGenerator : IRandomPositionGenerator
    {
        private readonly IRandomNumberGenerator randomGenerator;

        public RandomPositionGenerator(IRandomNumberGenerator randomGenerator)
        {
            this.randomGenerator = randomGenerator;
        }

        public Position GetNext(Size validArea)
        {
            var x = (uint) randomGenerator.Next(0, (int) validArea.Width - 1);
            var y = (uint) randomGenerator.Next(0, (int) validArea.Height - 1);

            return new Position(x, y);
        }
    }
}