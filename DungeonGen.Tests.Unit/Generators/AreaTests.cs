using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class AreaTests
    {
        private Area area;

        [SetUp]
        public void Setup()
        {
            area = new Area();
        }

        [Test]
        public void AreaInitialized()
        {
            Assert.That(area.Contents, Is.Not.Null);
            Assert.That(area.Descriptions, Is.Empty);
            Assert.That(area.Length, Is.EqualTo(0));
            Assert.That(area.Width, Is.EqualTo(0));
            Assert.That(area.Type, Is.Empty);
        }
    }
}
