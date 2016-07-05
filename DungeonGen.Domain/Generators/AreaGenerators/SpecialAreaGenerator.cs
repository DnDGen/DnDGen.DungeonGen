using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class SpecialAreaGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private IPercentileSelector percentileSelector;
        private PoolGenerator poolGenerator;
        private AreaGenerator caveGenerator;

        public SpecialAreaGenerator(IAreaPercentileSelector areaPercentileSelector, IPercentileSelector percentileSelector, PoolGenerator poolGenerator, AreaGenerator caveGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.percentileSelector = percentileSelector;
            this.poolGenerator = poolGenerator;
            this.caveGenerator = caveGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var shape = percentileSelector.SelectFrom(TableNameConstants.SpecialAreaShapes);

            if (shape == AreaTypeConstants.Cave)
                return caveGenerator.Generate(dungeonLevel, partyLevel);

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
                area.Contents.Pool = poolGenerator.Generate(partyLevel);
            }

            return new[] { area };
        }
    }
}
