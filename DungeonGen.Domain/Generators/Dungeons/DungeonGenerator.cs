using DnDGen.Core.Selectors.Percentiles;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.Dungeons
{
    internal class DungeonGenerator : IDungeonGenerator
    {
        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly AreaGeneratorFactory areaGeneratorFactory;
        private readonly ITrapGenerator trapGenerator;
        private readonly IPercentileSelector percentileSelector;
        private readonly IEnumerable<string> waterContents;
        private readonly IEncounterGenerator encounterGenerator;

        public DungeonGenerator(IAreaPercentileSelector areaPercentileSelector,
            AreaGeneratorFactory areaGeneratorFactory,
            ITrapGenerator trapGenerator,
            IPercentileSelector percentileSelector,
            IEncounterGenerator encounterGenerator)
        {
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.areaPercentileSelector = areaPercentileSelector;
            this.trapGenerator = trapGenerator;
            this.percentileSelector = percentileSelector;
            this.encounterGenerator = encounterGenerator;

            waterContents = new[]
            {
                ContentsConstants.MagicPool,
                ContentsConstants.River,
                ContentsConstants.Stream,
                ContentsConstants.TeleportationPool,
                ContentsTypeConstants.Lake,
                ContentsTypeConstants.Pool,
            };
        }

        public IEnumerable<Area> GenerateFromDoor(int dungeonLevel, EncounterSpecifications environment)
        {
            return Generate(dungeonLevel, environment, TableNameConstants.DungeonAreaFromDoor);
        }

        public IEnumerable<Area> GenerateFromHall(int dungeonLevel, EncounterSpecifications environment)
        {
            return Generate(dungeonLevel, environment, TableNameConstants.DungeonAreaFromHall);
        }

        private IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment, string tableName)
        {
            var areas = new List<Area>();
            var newAreas = GenerateAreas(dungeonLevel, environment, tableName);
            areas.AddRange(newAreas);

            while (areas.Last().Type == AreaTypeConstants.General)
            {
                newAreas = GenerateAreas(dungeonLevel, environment, tableName);
                areas.AddRange(newAreas);
            }

            var specificAreas = areas.Where(a => a.Type != AreaTypeConstants.General);
            var doorsAreFromHall = specificAreas.All(a => a.Type == AreaTypeConstants.Door);

            if (specificAreas.Count() == 1)
            {
                var area = specificAreas.Single();
                if (area.Type == AreaTypeConstants.Hall && tableName == TableNameConstants.DungeonAreaFromHall)
                {
                    area.Width = 0;
                }
            }

            foreach (var area in areas)
            {
                if (doorsAreFromHall && area.Type == AreaTypeConstants.Door)
                {
                    var doorLocation = percentileSelector.SelectFrom(TableNameConstants.DoorLocations);
                    area.Descriptions = area.Descriptions.Union(new[] { doorLocation });
                }

                area.Contents.Encounters = GenerateEncounters(area, environment);
                area.Contents.Traps = GenerateTraps(area, environment.Level);
            }

            if (doorsAreFromHall && areas.Any(a => a.Descriptions.Contains(DescriptionConstants.StraightAhead)) == false)
            {
                var continuingHall = new Area();
                continuingHall.Length = 30;
                continuingHall.Type = AreaTypeConstants.Hall;

                areas.Add(continuingHall);
            }

            return areas;
        }

        private IEnumerable<Area> GenerateAreas(int dungeonLevel, EncounterSpecifications environment, string tableName)
        {
            var area = areaPercentileSelector.SelectFrom(tableName);

            if (NeedToGenerateNewHall(area, tableName))
            {
                var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);
                var newHall = hallGenerator.Generate(dungeonLevel, environment).Single();
                newHall.Descriptions = newHall.Descriptions.Union(area.Descriptions);

                return new[] { newHall };
            }

            if (areaGeneratorFactory.HasSpecificGenerator(area.Type) == false)
                return new[] { area };

            var areaGenerator = areaGeneratorFactory.Build(area.Type);
            var specificAreas = new List<Area>();

            while (area.Width-- > 0)
            {
                var newAreas = areaGenerator.Generate(dungeonLevel, environment);
                specificAreas.AddRange(newAreas);
            }

            return specificAreas;
        }

        private bool NeedToGenerateNewHall(Area area, string tablename)
        {
            return area.Type == AreaTypeConstants.Hall && tablename == TableNameConstants.DungeonAreaFromDoor;
        }

        private IEnumerable<Encounter> GenerateEncounters(Area area, EncounterSpecifications environment)
        {
            var encounters = new List<Encounter>();
            var encounterContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Encounter);

            if (!encounterContents.Any())
                return encounters;

            foreach (var encounterContent in encounterContents)
            {
                var specifications = environment.Clone();
                specifications.AllowAquatic = area.Contents.Miscellaneous.Intersect(waterContents).Any();

                var encounter = encounterGenerator.Generate(specifications);
                encounters.Add(encounter);
            }

            return encounters;
        }

        private IEnumerable<Trap> GenerateTraps(Area area, int partyLevel)
        {
            var traps = new List<Trap>();
            var trapContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Trap);

            foreach (var trapContent in trapContents)
            {
                var trap = trapGenerator.Generate(partyLevel);
                traps.Add(trap);
            }

            return traps;
        }
    }
}
