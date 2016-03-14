using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class TurnGenerator : AreaGenerator
    {
        private IPercentileSelector percentileSelector;

        public TurnGenerator(IPercentileSelector percentileSelector)
        {
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var description = percentileSelector.SelectFrom(TableNameConstants.Turns);

            var turn = new Area();
            turn.Type = AreaTypeConstants.Turn;
            turn.Descriptions = new[] { description };
            turn.Length = 30;

            return new[] { turn };
        }
    }
}
