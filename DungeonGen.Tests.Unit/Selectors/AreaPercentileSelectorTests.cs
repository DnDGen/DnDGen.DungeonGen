using DungeonGen.Domain.Selectors;
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

        private PartialRoll ParseRoll(string roll)
        {
            var result = 0;
            if (!int.TryParse(roll, out result))
                throw new ArgumentException(roll + " is not set up to be rolled");

            var mockPartialRoll = new Mock<PartialRoll>();
            mockPartialRoll.Setup(r => r.AsSum()).Returns(result);

            return mockPartialRoll.Object;
        }

        private void SetUpRoll(string roll, int result)
        {
            var mockPartialRoll = new Mock<PartialRoll>();
            mockPartialRoll.Setup(r => r.AsSum()).Returns(result);
            mockDice.Setup(d => d.Roll(roll)).Returns(mockPartialRoll.Object);
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
        public void BUG_DoNotReplaceRollsInDescriptions()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type(description of 1d2 things/other description of 3d4 things)");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Contains.Item("description of 1d2 things"));
            Assert.That(area.Descriptions, Contains.Item("other description of 3d4 things"));
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
            mockDice.Setup(d => d.Roll("1d2").AsSum()).Returns(9266);
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
            mockDice.Setup(d => d.Roll("3d4").AsSum()).Returns(90210);
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
        public void ReturnNumericContentsFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type[42/600]");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("42"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("600"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(2));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasures, Is.Empty);
        }

        [Test]
        public void ReturnMultipleNumericContentsFromInnerSelector()
        {
            mockInnerSelector.Setup(s => s.SelectFrom("table name")).Returns("area type[42/600/even more contents]");

            var area = areaPercentileSelector.SelectFrom("table name");
            Assert.That(area, Is.Not.Null);
            Assert.That(area.Type, Is.EqualTo("area type"));
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Contents.Encounters, Is.Empty);
            Assert.That(area.Contents.IsEmpty, Is.False);
            Assert.That(area.Contents.Miscellaneous, Contains.Item("42"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("600"));
            Assert.That(area.Contents.Miscellaneous, Contains.Item("even more contents"));
            Assert.That(area.Contents.Miscellaneous.Count(), Is.EqualTo(3));
            Assert.That(area.Contents.Traps, Is.Empty);
            Assert.That(area.Contents.Treasures, Is.Empty);
        }

        [Test]
        public void ReturnFullAreaFromInnerSelector()
        {
            SetUpRoll("1d2", 9266);
            SetUpRoll("3d4", 90210);

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
