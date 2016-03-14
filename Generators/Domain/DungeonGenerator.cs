using DungeonGen.Common;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Generators.Domain
{
    public class DungeonGenerator : IDungeonGenerator
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

        public IEnumerable<Area> GenerateFromDoor(int level)
        {
            return Generate(level, TableNameConstants.DungeonAreaFromDoor);
        }

        public IEnumerable<Area> GenerateFromHall(int level)
        {
            return Generate(level, TableNameConstants.DungeonAreaFromHall);
        }

        private IEnumerable<Area> Generate(int level, string tableName)
        {
            var areas = GenerateAreas(level, tableName);

            while (areas.Last().Type == AreaTypeConstants.General)
            {
                var newAreas = GenerateAreas(level, tableName);
                areas = areas.Union(newAreas);
            }

            foreach (var area in areas)
            {
                if (area.Type == AreaTypeConstants.Door)
                {
                    var doorLocation = percentileSelector.SelectFrom(TableNameConstants.DoorLocations);
                    area.Descriptions = area.Descriptions.Union(new[] { doorLocation });
                }

                area.Contents.Encounters = GenerateEncounters(area, level);
                area.Contents.Traps = GenerateTraps(area, level);
            }

            return areas;
        }

        private IEnumerable<Area> GenerateAreas(int level, string tableName)
        {
            var area = areaPercentileSelector.SelectFrom(tableName);

            if (areaGeneratorFactory.HasSpecificGenerator(area.Type) == false)
                return new[] { area };

            var areaGenerator = areaGeneratorFactory.Build(area.Type);
            var specificAreas = new List<Area>();

            while (area.Width-- > 0)
            {
                var newAreas = areaGenerator.Generate(level);
                specificAreas.AddRange(newAreas);
            }

            return specificAreas;
        }

        private IEnumerable<Encounter> GenerateEncounters(Area area, int level)
        {
            var encounters = new List<Encounter>();
            var encounterContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Encounter);

            foreach (var encounterContent in encounterContents)
            {
                var encounter = encounterGenerator.Generate(EnvironmentConstants.Dungeon, level);
                encounters.Add(encounter);
            }

            return encounters;
        }

        private IEnumerable<Trap> GenerateTraps(Area area, int level)
        {
            var traps = new List<Trap>();
            var trapContents = area.Contents.Miscellaneous.Where(m => m == ContentsTypeConstants.Trap);

            foreach (var trapContent in trapContents)
            {
                var trap = trapGenerator.Generate(level);
                traps.Add(trap);
            }

            return traps;
        }
    }
}
