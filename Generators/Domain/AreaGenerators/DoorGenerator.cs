using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class DoorGenerator : AreaGenerator
    {
        private IPercentileSelector percentileSelector;

        public DoorGenerator(IPercentileSelector percentileSelector)
        {
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var description = percentileSelector.SelectFrom(TableNameConstants.DoorType);

            var door = new Area();
            door.Type = AreaTypeConstants.Door;
            door.Descriptions = new[] { description };

            return new[] { door };
        }
    }
}
