using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Selectors;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Selectors.Percentiles;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.ExitGenerators
{
    internal class RoomExitGenerator : ExitGenerator
    {
        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public RoomExitGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGeneratorFactory areaGeneratorFactory, IPercentileSelector percentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment, int length, int width)
        {
            var selectedExit = areaPercentileSelector.SelectFrom(TableNameConstants.RoomExits);

            if (selectedExit.Length > 0 && length * width > selectedExit.Length)
                selectedExit.Width++;

            var exits = new List<Area>();

            while (selectedExit.Width-- > 0)
            {
                var exit = GetExit(dungeonLevel, environment, selectedExit.Type);
                var additionalDescriptions = GetDescription(exit);
                exit.Descriptions = exit.Descriptions.Union(additionalDescriptions);

                exits.Add(exit);
            }

            return exits;
        }

        private Area GetExit(int dungeonLevel, EncounterSpecifications environment, string exitType)
        {
            var exitGenerator = areaGeneratorFactory.Build(exitType);
            return exitGenerator.Generate(dungeonLevel, environment).Single();
        }

        private IEnumerable<string> GetDescription(Area exit)
        {
            var location = percentileSelector.SelectFrom(Config.Name, TableNameConstants.ExitLocation);

            if (exit.Type == AreaTypeConstants.Door)
                return new[] { location };

            var direction = percentileSelector.SelectFrom(Config.Name, TableNameConstants.ExitDirection);
            return new[] { location, direction };
        }
    }
}
