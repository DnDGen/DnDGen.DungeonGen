using DungeonGen.Selectors;
using DungeonGen.Selectors.Domain;
using Moq;
using NUnit.Framework;
using RollGen;
using System;
using System.Linq;

namespace DungeonGen.Tests.Unit.Selectors
{
    [TestFixture]
    public class AreaPercentileSelectorTests
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private Mock<IPercentileSelector> mockInnerSelector;
        private Mock<Dice> mockDice;

        [SetUp]
        public void Setup()
        {
            mockInnerSelector = new Mock<IPercentileSelector>();
            mockDice = new Mock<Dice>();
            areaPercentileSelector = new AreaPercentileSelector(mockInnerSelector.Object, mockDice.Object);

            mockDice.Setup(d => d.Roll(It.IsAny<string>())).Returns((string s) => ParseRoll(s));
        }

        private int ParseRoll(string s)
        {
            var result = 0;
            if (int.TryParse(s, out result))
                return result;

            throw new ArgumentException(s + " is not set up to be rolled");
        }

        [Test]
        public void ReturnAreaTypeFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
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
            Assert.That(area.Descriptions.Single(), Is.EqualTo("description"));
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnMultipleDescriptionsFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type(description/other description)");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Contains.Item("description"));
            Assert.That(area.Descriptions, Contains.Item("other description"));
            Assert.That(area.Descriptions.Count(), Is.EqualTo(2));
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
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(9266));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnRandomLength()
        {
            mockDice.Setup(d => d.Roll("1d2")).Returns(9266);
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type{1d2x0}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
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
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(90210));
            Assert.That(area.Contents.IsEmpty, Is.True);
        }

        [Test]
        public void ReturnRandomWidth()
        {
            mockDice.Setup(d => d.Roll("3d4")).Returns(90210);
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type{0x3d4}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
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
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(1));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasures, Is.Empty);
        }

        [Test]
        public void ReturnMultipleContentsFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type[contents/more contents/even more contents]");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("contents"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("more contents"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("even more contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(3));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasures, Is.Empty);
        }

        [Test]
        public void ReturnFullAreaFromInnerSelector()
        {
            mockDice.Setup(d => d.Roll("1d2")).Returns(9266);
            mockDice.Setup(d => d.Roll("3d4")).Returns(90210);
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type(description/other description)[contents/more contents/even more contents]{1d2x3d4}");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Contains.Item("description"));
            Assert.That(area.Descriptions, Contains.Item("other description"));
            Assert.That(area.Descriptions.Count(), Is.EqualTo(2));
            Assert.That(area.Length, Is.EqualTo(9266));
            Assert.That(area.Width, Is.EqualTo(90210));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("contents"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("more contents"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("even more contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(3));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasures, Is.Empty);
        }
    }
}
