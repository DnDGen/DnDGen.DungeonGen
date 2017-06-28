using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.ExitGenerators
{
    internal class ChamberExitGenerator : ExitGenerator
    {
        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public ChamberExitGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGeneratorFactory areaGeneratorFactory, IPercentileSelector percentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment, int length, int width)
        {
            var selectedExit = areaPercentileSelector.SelectFrom(TableNameConstants.ChamberExits);

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
            var location = percentileSelector.SelectFrom(TableNameConstants.ExitLocation);

            if (exit.Type == AreaTypeConstants.Door)
                return new[] { location };

            var direction = percentileSelector.SelectFrom(TableNameConstants.ExitDirection);
            return new[] { location, direction };
        }
    }
}
