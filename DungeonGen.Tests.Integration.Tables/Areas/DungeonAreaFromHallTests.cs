using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Tables;
using Ninject;
using NUnit.Framework;
using System;

namespace DungeonGen.Tests.Integration.Tables.Areas
{
    [TestFixture]
    public class DungeonAreaFromHallTests : AreaPercentileTests
    {
        [Inject]
        internal AreaGeneratorFactory AreaGeneratorFactory { get; set; }

        protected override string tableName
        {
            get
            {
                return TableNameConstants.DungeonAreaFromHall;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 10, AreaTypeConstants.Hall, "", "", 30, 0)]
        [TestCase(26, 50, AreaTypeConstants.SidePassage, "", "", 30, 1)]
        [TestCase(51, 65, AreaTypeConstants.Turn, "", "", 30, 1)]
        [TestCase(66, 80, AreaTypeConstants.Chamber, "", "", 0, 1)]
        [TestCase(81, 85, AreaTypeConstants.Stairs, "", "", 0, 1)]
        [TestCase(86, 90, AreaTypeConstants.DeadEnd, "Check for secret doors along already mapped walls", "", 0, 0)]
        [TestCase(91, 95, AreaTypeConstants.General, "", ContentsTypeConstants.Trap, 0, 0)]
        [TestCase(96, 100, AreaTypeConstants.General, "", ContentsTypeConstants.Encounter, 0, 0)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);

            var expectedWidth = Convert.ToInt32(AreaGeneratorFactory.HasSpecificGenerator(areaType));
            Assert.That(width, Is.EqualTo(expectedWidth));
        }

        [TestCase(11, 25, AreaTypeConstants.Door, "", "", 0, "1d3")]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, string width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);
        }
    }
}
