using DungeonGen.Domain.Generators.RuntimeFactories;
using DungeonGen.Domain.Tables;
using Ninject;
using NUnit.Framework;
using System;

namespace DungeonGen.Tests.Integration.Tables.Areas
{
    [TestFixture]
    public class DungeonAreaFromDoorTests : AreaPercentileTests
    {
        [Inject]
        internal IAreaGeneratorFactory AreaGeneratorFactory { get; set; }

        protected override string tableName
        {
            get
            {
                return TableNameConstants.DungeonAreaFromDoor;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 20, SidePassageConstants.ParallelPassage, "", "", 30, 1)]
        [TestCase(21, 40, AreaTypeConstants.Hall, "Straight ahead", "", 30, 0)]
        [TestCase(41, 45, AreaTypeConstants.Hall, "45 degrees left", "", 30, 0)]
        [TestCase(46, 50, AreaTypeConstants.Hall, "45 degrees right", "", 30, 0)]
        [TestCase(51, 90, AreaTypeConstants.Room, "", "", 0, 1)]
        [TestCase(91, 100, AreaTypeConstants.Chamber, "", "", 0, 1)]
        public override void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            base.AreaPercentile(lower, upper, areaType, description, contents, length, width);

            var expectedWidth = Convert.ToInt32(AreaGeneratorFactory.HasSpecificGenerator(areaType));
            Assert.That(width, Is.EqualTo(expectedWidth));
        }
    }
}
