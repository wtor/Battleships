using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battleships.Core.Common;
using NUnit.Framework;

namespace Battleships.Core.Tests.Common
{
    public sealed class SizeTests
    {
        [Test]
        public void GetPositionsStartingFrom_returns_no_position_when_size_is_0_0()
        {
            var size = new Size(0, 0);

            var positions = size.GetPositionsStartingFrom(new Position(10, 10));

            Assert.That(positions, Is.Empty);
        }

        [Test]
        [TestCaseSource(nameof(GetPositionsStartingFrom_returns_valid_positions_Source))]
        public IReadOnlyList<Position> GetPositionsStartingFrom_returns_valid_positions(Size size, Position startingPosition)
        {
            return size.GetPositionsStartingFrom(startingPosition).ToList();
        }

        private static IEnumerable GetPositionsStartingFrom_returns_valid_positions_Source()
        {
            yield return new TestCaseData(new Size(1, 1), new Position(0, 0))
                .Returns(new[]
                {
                    new Position(0, 0)
                });

            yield return new TestCaseData(new Size(1, 3), new Position(0, 0))
                .Returns(new[]
                {
                    new Position(0, 0),
                    new Position(0, 1),
                    new Position(0, 2),
                });

            yield return new TestCaseData(new Size(4, 1), new Position(3, 3))
                .Returns(new[]
                {
                    new Position(3, 3),
                    new Position(4, 3),
                    new Position(5, 3),
                    new Position(6, 3),
                });
        }
    }
}