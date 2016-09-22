using DungeonGen.Domain.Generators.RuntimeFactories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators
{
    internal class DungeonGenerator : IDungeonGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private IAreaGeneratorFactory areaGeneratorFactory;
        private IEncounterGenerator encounterGenerator;
        private ITrapGenerator trapGenerator;
        private IPercentileSelector percentileSelector;
        private AreaGenerator hallGenerator;

        public DungeonGenerator(IAreaPercentileSelector areaPercentileSelector, IAreaGeneratorFactory areaGeneratorFactory, IEncounterGenerator encounterGenerator, ITrapGenerator trapGenerator, IPercentileSelector percentileSelector, AreaGenerator hallGenerator)
        {
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.areaPercentileSelector = areaPercentileSelector;
            this.encounterGenerator = encounterGenerator;
            this.trapGenerator = trapGenerator;
            this.percentileSelector = percentileSelector;
            this.hallGenerator = hallGenerator;
        }

        public IEnumerable<Area> GenerateFromDoor(int dungeonLevel, int partyLevel, string temperature)
        {
            return Generate(dungeonLevel, partyLevel, TableNameConstants.DungeonAreaFromDoor, temperature);
        }

        public IEnumerable<Area> GenerateFromHall(int dungeonLevel, int partyLevel, string temperature)
        {
            return Generate(dungeonLevel, partyLevel, TableNameConstants.DungeonAreaFromHall, temperature);
        }

        private IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string tableName, string temperature)
        {
            var areas = GenerateAreas(dungeonLevel, partyLevel, tableName, temperature);

            while (areas.Last().Type == AreaTypeConstants.General)
            {
                var newAreas = GenerateAreas(dungeonLevel, partyLevel, tableName, temperature);
                areas = areas.Union(newAreas);
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

                area.Contents.Encounters = GenerateEncounters(area, partyLevel, temperature);
                area.Contents.Traps = GenerateTraps(area, partyLevel);
            }

            if (doorsAreFromHall && areas.Any(a => a.Descriptions.Contains(DescriptionConstants.StraightAhead)) == false)
            {
                var continuingHall = new Area();
                continuingHall.Length = 30;
                continuingHall.Type = AreaTypeConstants.Hall;

                areas = areas.Union(new[] { continuingHall });
            }

            return areas;
        }

        private IEnumerable<Area> GenerateAreas(int dungeonLevel, int partyLevel, string tableName, string temperature)
        {
            var area = areaPercentileSelector.SelectFrom(tableName);

            if (NeedToGenerateNewHall(area, tableName))
            {
                var newHall = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
                newHall.Descriptions = newHall.Descriptions.Union(area.Descriptions);

                return new[] { newHall };
            }

            if (areaGeneratorFactory.HasSpecificGenerator(area.Type) == false)
                return new[] { area };

            var areaGenerator = areaGeneratorFactory.Build(area.Type);
            var specificAreas = new List<Area>();

            while (area.Width-- > 0)
            {
                var newAreas = areaGenerator.Generate(dungeonLevel, partyLevel, temperature);
                specificAreas.AddRange(newAreas);
            }

            return specificAreas;
        }

        private bool NeedToGenerateNewHall(Area area, string tablename)
        {
            return area.Type == AreaTypeConstants.Hall && tablename == TableNameConstants.DungeonAreaFromDoor;
        }

        private IEnumerable<Encounter> GenerateEncounters(Area area, int partyLevel, string temperature)
        {
            var encounters = new List<Encounter>();
            var encounterContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Encounter);

            foreach (var encounterContent in encounterContents)
            {
                var encounter = encounterGenerator.Generate(EnvironmentConstants.Dungeon, partyLevel, temperature, EnvironmentConstants.TimesOfDay.Night);
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
