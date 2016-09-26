using DungeonGen.Domain.Mappers;
using RollGen;
using System;

namespace DungeonGen.Domain.Selectors
{
    internal class PercentileSelector : IPercentileSelector
    {
        private PercentileMapper percentileMapper;
        private Dice dice;

        public PercentileSelector(PercentileMapper percentileMapper, Dice dice)
        {
            this.percentileMapper = percentileMapper;
            this.dice = dice;
        }

        public string SelectFrom(string tableName)
        {
            var table = percentileMapper.Map(tableName);
            var roll = dice.Roll().Percentile();

            if (table.ContainsKey(roll) == false)
            {
                var message = string.Format("{0} is not a valid entry in the table {1}", roll, tableName);
                throw new ArgumentException(message);
            }

            return table[roll];
        }
    }
}
