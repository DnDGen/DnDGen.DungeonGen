using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
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

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, int length, int width, string temperature)
        {
            var selectedExit = areaPercentileSelector.SelectFrom(TableNameConstants.ChamberExits);

            if (selectedExit.Length > 0 && length * width > selectedExit.Length)
                selectedExit.Width++;

            var exits = new List<Area>();

            while (selectedExit.Width-- > 0)
            {
                var exit = GetExit(dungeonLevel, partyLevel, selectedExit.Type, temperature);
                var additionalDescriptions = GetDescription(exit);
                exit.Descriptions = exit.Descriptions.Union(additionalDescriptions);

                exits.Add(exit);
            }

            return exits;
        }

        private Area GetExit(int dungeonLevel, int partyLevel, string type, string temperature)
        {
            var exitGenerator = areaGeneratorFactory.Build(type);
            return exitGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
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
