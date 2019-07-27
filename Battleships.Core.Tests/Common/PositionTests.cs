using System.Collections;
using Battleships.Core.Common;
using NUnit.Framework;

namespace Battleships.Core.Tests.Common
{
    public sealed class PositionTests
    {
        [Test]
        [TestCaseSource(nameof(IsIncludedIn_works_correctly_when_size_dimension_is_equal_to_position_coordinate_Source))]
        public void IsIncludedIn_works_correctly_when_size_dimension_is_equal_to_position_coordinate(Position position)
        {
            var size = new Size(8, 8);

            var actual = position.IsIncludedIn(size);

            Assert.That(actual, Is.False);
        }

        private static IEnumerable IsIncludedIn_works_correctly_when_size_dimension_is_equal_to_position_coordinate_Source()
        {
            yield return new Position(7, 8);
            yield return new Position(8, 7);
        }

        [Test]
        public void IsIncludedIn_works_correctly_when_size_dimension_is_less_than_position_coordinate()
        {
            var position = new Position(7, 7);
            var size = new Size(8, 8);

            var actual = position.IsIncludedIn(size);

            Assert.That(actual, Is.True);
        }
    }
}