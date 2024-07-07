using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Selectors.Percentiles;
using System.Collections.Generic;

namespace DnDGen.DungeonGen.Generators.AreaGenerators
{
    internal class TurnGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Turn; }
        }

        private readonly IPercentileSelector percentileSelector;

        public TurnGenerator(IPercentileSelector percentileSelector)
        {
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var description = percentileSelector.SelectFrom(Config.Name, TableNameConstants.Turns);

            var turn = new Area
            {
                Type = AreaTypeConstants.Turn,
                Descriptions = [description],
                Length = 30
            };

            return [turn];
        }
    }
}
