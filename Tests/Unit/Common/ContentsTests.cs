using DungeonGen.Common;
using EncounterGen.Common;
using NUnit.Framework;
using TreasureGen.Common.Goods;
using TreasureGen.Common.Items;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContentsTests
    {
        private Contents contents;

        [SetUp]
        public void Setup()
        {
            contents = new Contents();
        }

        [Test]
        public void ContentsInitialized()
        {
            Assert.That(contents.Encounters, Is.Empty);
            Assert.That(contents.Miscellaneous, Is.Empty);
            Assert.That(contents.Treasure, Is.Not.Null);
            Assert.That(contents.TreasureContainer, Is.Empty);
            Assert.That(contents.Traps, Is.Empty);
        }

        [Test]
        public void IsNotEmptyIfEncounter()
        {
            contents.Encounters = new[] { new Encounter() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfMiscellaneous()
        {
            contents.Miscellaneous = new[] { "misc" };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTrap()
        {
            contents.Traps = new[] { new Trap() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTreasureCoin()
        {
            contents.Treasure.Coin.Quantity = 1;
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTreasureGoods()
        {
            contents.Treasure.Goods = new[] { new Good() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTreasureItems()
        {
            contents.Treasure.Items = new[] { new Item() };
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsNotEmptyIfTreasureContainer()
        {
            contents.TreasureContainer = "container";
            Assert.That(contents.IsEmpty, Is.False);
        }

        [Test]
        public void IsEmptyIfEmpty()
        {
            Assert.That(contents.IsEmpty, Is.True);
        }
    }
}
