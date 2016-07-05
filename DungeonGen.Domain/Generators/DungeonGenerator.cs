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

        public DungeonGenerator(IAreaPercentileSelector areaPercentileSelector, IAreaGeneratorFactory areaGeneratorFactory, IEncounterGenerator encounterGenerator, ITrapGenerator trapGenerator, IPercentileSelector percentileSelector)
        {
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.areaPercentileSelector = areaPercentileSelector;
            this.encounterGenerator = encounterGenerator;
            this.trapGenerator = trapGenerator;
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> GenerateFromDoor(int dungeonLevel, int partyLevel)
        {
            return Generate(dungeonLevel, partyLevel, TableNameConstants.DungeonAreaFromDoor);
        }

        public IEnumerable<Area> GenerateFromHall(int dungeonLevel, int partyLevel)
        {
            return Generate(dungeonLevel, partyLevel, TableNameConstants.DungeonAreaFromHall);
        }

        private IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string tableName)
        {
            var areas = GenerateAreas(dungeonLevel, partyLevel, tableName);

            while (areas.Last().Type == AreaTypeConstants.General)
            {
                var newAreas = GenerateAreas(dungeonLevel, partyLevel, tableName);
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

                area.Contents.Encounters = GenerateEncounters(area, partyLevel);
                area.Contents.Traps = GenerateTraps(area, partyLevel);
            }

            return areas;
        }

        private IEnumerable<Area> GenerateAreas(int dungeonLevel, int partyLevel, string tableName)
        {
            var area = areaPercentileSelector.SelectFrom(tableName);

            if (areaGeneratorFactory.HasSpecificGenerator(area.Type) == false)
                return new[] { area };

            var areaGenerator = areaGeneratorFactory.Build(area.Type);
            var specificAreas = new List<Area>();

            while (area.Width-- > 0)
            {
                var newAreas = areaGenerator.Generate(dungeonLevel, partyLevel);
                specificAreas.AddRange(newAreas);
            }

            return specificAreas;
        }

        private IEnumerable<Encounter> GenerateEncounters(Area area, int partyLevel)
        {
            var encounters = new List<Encounter>();
            var encounterContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Encounter);

            foreach (var encounterContent in encounterContents)
            {
                var encounter = encounterGenerator.Generate(EnvironmentConstants.Dungeon, partyLevel);
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
