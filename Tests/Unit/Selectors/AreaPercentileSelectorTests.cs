using DungeonGen.Selectors;
using DungeonGen.Selectors.Domain;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DungeonGen.Tests.Unit.Selectors
{
    [TestFixture]
    public class AreaPercentileSelectorTests
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private Mock<IPercentileSelector> mockInnerSelector;

        [SetUp]
        public void Setup()
        {
            mockInnerSelector = new Mock<IPercentileSelector>();
            areaPercentileSelector = new AreaPercentileSelector(mockInnerSelector.Object);
        }

        [Test]
        public void ReturnAreaTypeFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnDescriptionFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type(description)");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.EqualTo("description"));
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnLengthFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type{9266x0}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(9266));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnWidthFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type{0x90210}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(90210));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnContentsFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type[contents]");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(1));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasure.Coin.Quantity, Is.EqualTo(0));
            Assert.That(area.Contents.Treasure.Goods, Is.Empty);
            Assert.That(area.Contents.Treasure.Items, Is.Empty);
            Assert.That(area.Contents.TreasureContainer, Is.Empty);
        }

        [Test]
        public void ReturnFullAreaFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type(description)[contents]{9266x90210}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Description, Is.EqualTo("description"));
            Assert.That(area.Length, Is.EqualTo(9266));
            Assert.That(area.Width, Is.EqualTo(90210));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(1));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasure.Coin.Quantity, Is.EqualTo(0));
            Assert.That(area.Contents.Treasure.Goods, Is.Empty);
            Assert.That(area.Contents.Treasure.Items, Is.Empty);
            Assert.That(area.Contents.TreasureContainer, Is.Empty);
        }
    }
}
