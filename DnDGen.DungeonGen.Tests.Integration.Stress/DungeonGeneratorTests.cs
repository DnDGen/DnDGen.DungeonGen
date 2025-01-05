using DnDGen.DungeonGen.Generators;
using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Selectors.Collections;
using DnDGen.RollGen;
using NUnit.Framework;
using System.Collections.Generic;

namespace DnDGen.DungeonGen.Tests.Integration.Stress
{
    [TestFixture]
    public class DungeonGeneratorTests : StressTests
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

        [Test]
        public void StressDungeonGeneratorFromDoor()
        {
            stressor.Stress(() => AssertRandomArea(false));
        }

        [Test]
        public void StressDungeonGeneratorFromHall()
        {
            stressor.Stress(() => AssertRandomArea(true));
        }

        protected void AssertRandomArea(bool fromHall)
        {
            var areas = GenerateAreas(fromHall);
            areaAsserter.AssertAreas(areas);
        }

        private IEnumerable<Area> GenerateAreas(bool fromHall)
        {
            var dungeonLevel = dice.Roll().d20().AsSum();
            var specifications = RandomizeSpecifications();

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
    }
}
