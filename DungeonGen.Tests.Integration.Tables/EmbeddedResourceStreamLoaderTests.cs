using DungeonGen.Domain.Tables;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace DungeonGen.Tests.Integration.Tables
{
    [TestFixture]
    public class EmbeddedResourceStreamLoaderTests : IntegrationTests
    {
        [Inject]
        internal StreamLoader StreamLoader { get; set; }

        [Test]
        public void GetsFileIfItIsAnEmbeddedResource()
        {
            var table = Load("DungeonAreaFromHall.xml");

            for (var i = 100; i > 95; i--)
                Assert.That(table[i], Is.EqualTo("General[Encounter]{0x1}"));

            for (var i = 95; i > 90; i--)
                Assert.That(table[i], Is.EqualTo("General[Trap]{0x1}"));

            for (var i = 90; i > 85; i--)
                Assert.That(table[i], Is.EqualTo("Dead end(Check for secret doors along already mapped walls){0x1}"));

            for (var i = 85; i > 80; i--)
                Assert.That(table[i], Is.EqualTo("Stairs{0x1}"));

            for (var i = 80; i > 65; i--)
                Assert.That(table[i], Is.EqualTo("Chamber{0x1}"));

            for (var i = 65; i > 50; i--)
                Assert.That(table[i], Is.EqualTo("Turn{30x1}"));

            for (var i = 50; i > 25; i--)
                Assert.That(table[i], Is.EqualTo("Side passage{30x1}"));

            for (var i = 25; i > 10; i--)
                Assert.That(table[i], Is.EqualTo("Door{0x1d3}"));

            for (var i = 10; i > 0; i--)
                Assert.That(table[i], Is.EqualTo("Hall{30x1}"));
        }

        private Dictionary<int, string> Load(string filename)
        {
            var table = new Dictionary<int, string>();
            var xmlDocument = new XmlDocument();

            using (var stream = StreamLoader.LoadFor(filename))
                xmlDocument.Load(stream);

            var objects = xmlDocument.DocumentElement.ChildNodes;
            foreach (XmlNode node in objects)
            {
                var lower = Convert.ToInt32(node.SelectSingleNode("lower").InnerText);
                var upper = Convert.ToInt32(node.SelectSingleNode("upper").InnerText);
                var content = node.SelectSingleNode("content").InnerText;

                for (var i = lower; i <= upper; i++)
                    table[i] = content;
            }

            return table;
        }

        [Test]
        public void ThrowErrorIfFileIsNotFormattedCorrectly()
        {
            Assert.That(() => StreamLoader.LoadFor("DungeonAreaFromHall"), Throws.ArgumentException.With.Message.EqualTo("\"DungeonAreaFromHall\" is not a valid file"));
        }

        [Test]
        public void ThrowErrorIfFileIsNotAnEmbeddedResource()
        {
            Assert.That(() => StreamLoader.LoadFor("invalid filename.xml"), Throws.InstanceOf<FileNotFoundException>().With.Message.EqualTo("invalid filename.xml"));
        }

        [Test]
        public void MatchWholeFileName()
        {
            Assert.That(() => StreamLoader.LoadFor("FromHall.xml"), Throws.InstanceOf<FileNotFoundException>().With.Message.EqualTo("FromHall.xml"));
        }
    }
}
