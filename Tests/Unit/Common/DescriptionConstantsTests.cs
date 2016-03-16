using DungeonGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class DescriptionConstantsTests
    {
        [TestCase(DescriptionConstants.Chimney, "Chimney")]
        [TestCase(DescriptionConstants.Circular, "Circular")]
        [TestCase(DescriptionConstants.Iron, "Iron")]
        [TestCase(DescriptionConstants.MagicallyReinforced, "Magically reinforced")]
        [TestCase(DescriptionConstants.SlidesToSide, "Slides to one side")]
        [TestCase(DescriptionConstants.SlidesDown, "Slides down")]
        [TestCase(DescriptionConstants.SlidesUp, "Slides up")]
        [TestCase(DescriptionConstants.Stone, "Stone")]
        [TestCase(DescriptionConstants.TrapDoor, "TrapDoor")]
        [TestCase(DescriptionConstants.Wooden, "Wooden")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}
