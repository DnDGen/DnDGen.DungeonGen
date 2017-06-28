using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using System.Collections.Generic;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class TurnGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Turn; }
        }

        private IPercentileSelector percentileSelector;

        public TurnGenerator(IPercentileSelector percentileSelector)
        {
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
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
