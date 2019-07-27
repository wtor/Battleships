using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Services;
using NUnit.Framework;

namespace Battleships.Core.Tests.TestInfrastructure
{
    public sealed class DeterministicModuloRandomNumberGenerator : IRandomNumberGenerator
    {
        private int counter;

        public int Next(int min, int maxInclusive)
        {
            var diff = maxInclusive - min;

            var next = counter % (diff + 1) + min;

            counter++;

            return next;
        }
    }

    public sealed class DeterministicSequenceNumberGenerator : IRandomNumberGenerator
    {
        private readonly IReadOnlyList<int> sequence;
        private int counter;

        public DeterministicSequenceNumberGenerator(IReadOnlyList<int> sequence)
        {
            this.sequence = sequence;
        }

        public int Next(int min, int maxInclusive)
        {
            return sequence[counter++ % sequence.Count];
        }
    }

    public class DeterministicModuloRandomNumberGeneratorTests
    {
        [Test]
        public void Next_returns_values_in_expected_order()
        {
            var generator = new DeterministicModuloRandomNumberGenerator();

            var actual = Enumerable
                .Range(0, 10)
                .Select(i => generator.Next(2, 7));

            var expected = new[] {2, 3, 4, 5, 6, 7, 2, 3, 4, 5};

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }

    public class DeterministicSequenceNumberGeneratorTests
    {
        [Test]
        public void Next_returns_values_in_expected_order()
        {
            var generator = new DeterministicSequenceNumberGenerator(new[] {1, 3, 5});

            var actual = Enumerable
                .Range(0, 7)
                .Select(i => generator.Next(2, 7));

            var expected = new[] {1, 3, 5, 1, 3, 5, 1};

            Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}