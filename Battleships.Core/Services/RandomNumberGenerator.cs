using System;

namespace Battleships.Core.Services
{
    public interface IRandomNumberGenerator
    {
        int Next(int min, int maxInclusive);
    }

    public sealed class RandomNumberGenerator : IRandomNumberGenerator
    {
        private readonly Random random;

        public RandomNumberGenerator()
        {
            random = new Random();
        }

        public int Next(int min, int maxInclusive)
        {
            return random.Next(min, maxInclusive + 1);
        }
    }
}