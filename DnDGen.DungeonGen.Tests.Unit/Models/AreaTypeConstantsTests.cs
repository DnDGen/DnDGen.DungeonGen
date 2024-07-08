using DnDGen.DungeonGen.Models;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Unit.Models
{
    [TestFixture]
    public class AreaTypeConstantsTests
    {
        [TestCase(AreaTypeConstants.Cave, "Cave")]
        [TestCase(AreaTypeConstants.Chamber, "Chamber")]
        [TestCase(AreaTypeConstants.DeadEnd, "Dead end")]
        [TestCase(AreaTypeConstants.Door, "Door")]
        [TestCase(AreaTypeConstants.Hall, "Hall")]
        [TestCase(AreaTypeConstants.General, "General")]
        [TestCase(AreaTypeConstants.Room, "Room")]
        [TestCase(AreaTypeConstants.SidePassage, "Side passage")]
        [TestCase(AreaTypeConstants.Special, "Special")]
        [TestCase(AreaTypeConstants.Stairs, "Stairs")]
        [TestCase(AreaTypeConstants.Turn, "Turn")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}
