using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class DoorGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;

        public DoorGenerator(IAreaPercentileSelector areaPercentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var door = areaPercentileSelector.SelectFrom(TableNameConstants.DoorTypes);

            return new[] { door };
        }
    }
}
