using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Tests.Integration
{
    [TestFixture]
    public class DungeonGeneratorTests : IntegrationTests
    {
        private IDungeonGenerator dungeonGenerator;
        private Dice dice;
        private ICollectionSelector collectionSelector;
        private AreaAsserter areaAsserter;

        [SetUp]
        public void Setup()
        {
            dungeonGenerator = GetNewInstanceOf<IDungeonGenerator>();
            dice = GetNewInstanceOf<Dice>();
            collectionSelector = GetNewInstanceOf<ICollectionSelector>();
            areaAsserter = new AreaAsserter();
        }

        private IEnumerable<Area> GenerateAreas(bool fromHall, int presetPartyLevel = 0)
        {
            var dungeonLevel = dice.Roll().d20().AsSum();
            var specifications = RandomizeSpecifications(presetPartyLevel);

            if (fromHall)
                return dungeonGenerator.GenerateFromHall(dungeonLevel, specifications);

            return dungeonGenerator.GenerateFromDoor(dungeonLevel, specifications);
        }

        private EncounterSpecifications RandomizeSpecifications(int level = 0)
        {
            var specifications = new EncounterSpecifications
            {
                Environment = GetRandomFrom(AreaParameters.AllEnvironments),
                Temperature = GetRandomFrom(AreaParameters.AllTemperatures),
                TimeOfDay = GetRandomFrom(AreaParameters.AllTimesOfDay),
                Level = level > 0 ? level : dice.Roll().d20().AsSum(),
                AllowAquatic = dice.Roll().d2().AsTrueOrFalse(),
                AllowUnderground = dice.Roll().d2().AsTrueOrFalse()
            };

            return specifications;
        }

        private string GetRandomFrom(IEnumerable<string> collection)
        {
            return collectionSelector.SelectRandomFrom(collection);
        }

        [Repeat(100)]
        [Test]
        public void BUG_ContinuingHallHasSamePassageWidth()
        {
            var areas = GenerateAreas(true, 1);

            areaAsserter.AssertAreas(areas);

            if (areas.Count() == 1 && areas.Single().Type == AreaTypeConstants.Hall)
            {
                var hall = areas.Single();
                Assert.That(hall.Type == AreaTypeConstants.Hall);
                Assert.That(hall.Width, Is.Zero);
            }
        }
    }
}
