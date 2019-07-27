using Battleships.Core.Common;
using Battleships.Core.Ships;
using NUnit.Framework;

namespace Battleships.Core.Tests.Ships
{
    public sealed class ShipTests
    {
        [Test]
        public void When_ship_is_created_it_does_not_sink()
        {
            var destroyer = new HorizontalDestroyer(new Position(0, 0));

            Assert.That(destroyer.IsSunk, Is.False);
        }

        [Test]
        public void When_all_ship_parts_are_hit_then_ship_sinks()
        {
            var destroyer = new HorizontalDestroyer(new Position(0, 0));

            destroyer.GetShot(new Shot(new Position(0, 0)));
            Assert.That(destroyer.IsSunk, Is.False);

            destroyer.GetShot(new Shot(new Position(1, 0)));
            Assert.That(destroyer.IsSunk, Is.False);

            destroyer.GetShot(new Shot(new Position(2, 0)));
            Assert.That(destroyer.IsSunk, Is.False);

            destroyer.GetShot(new Shot(new Position(3, 0)));
            Assert.That(destroyer.IsSunk, Is.True);
        }
    }
}