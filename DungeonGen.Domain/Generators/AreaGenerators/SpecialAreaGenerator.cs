using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class SpecialAreaGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Special; }
        }

        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly PoolGenerator poolGenerator;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public SpecialAreaGenerator(IAreaPercentileSelector areaPercentileSelector, IPercentileSelector percentileSelector, PoolGenerator poolGenerator, AreaGeneratorFactory areaGeneratorFactory)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.percentileSelector = percentileSelector;
            this.poolGenerator = poolGenerator;
            this.areaGeneratorFactory = areaGeneratorFactory;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var shape = percentileSelector.SelectFrom(TableNameConstants.SpecialAreaShapes);

            if (shape == AreaTypeConstants.Cave)
            {
                var caveGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Cave);
                var caves = caveGenerator.Generate(dungeonLevel, environment);
                return caves;
            }

            var area = new Area();
            area.Width = 1;
            area.Descriptions = new[] { shape };

            var shouldReroll = false;

            do
            {
                var size = areaPercentileSelector.SelectFrom(TableNameConstants.SpecialAreaSizes);
                area.Length += size.Width + size.Length;
                shouldReroll = size.Width > 0;
            } while (shouldReroll);

            if (area.Descriptions.Contains(DescriptionConstants.Circular))
            {
                area.Contents.Pool = poolGenerator.Generate(environment);
            }

            return new[] { area };
        }
    }
}
