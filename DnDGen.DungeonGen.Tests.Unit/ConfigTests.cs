using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Unit
{
    internal class ConfigTests
    {
        [Test]
        public void ConfigNameIsCorrect()
        {
            var configType = typeof(Config);
            Assert.That(Config.Name, Is.EqualTo("DnDGen.DungeonGen").And.EqualTo(configType.Namespace));
        }
    }
}
