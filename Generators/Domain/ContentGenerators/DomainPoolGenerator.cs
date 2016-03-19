using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using EncounterGen.Common;
using EncounterGen.Generators;
using TreasureGen.Generators;

namespace DungeonGen.Generators.Domain.ContentGenerators
{
    public class DomainPoolGenerator : PoolGenerator
    {
        private IPercentileSelector percentileSelector;
        private IEncounterGenerator encounterGenerator;
        private ITreasureGenerator treasureGenerator;

        public DomainPoolGenerator(IPercentileSelector percentileSelector, IEncounterGenerator encounterGenerator, ITreasureGenerator treasureGenerator)
        {
            this.percentileSelector = percentileSelector;
            this.encounterGenerator = encounterGenerator;
            this.treasureGenerator = treasureGenerator;
        }

        public Pool Generate(int partyLevel)
        {
            var selectedPool = percentileSelector.SelectFrom(TableNameConstants.Pools);

            if (string.IsNullOrEmpty(selectedPool))
                return null;

            var pool = new Pool();
            var sections = selectedPool.Split('/');

            foreach (var section in sections)
            {
                if (section == ContentsTypeConstants.Encounter)
                {
                    pool.Encounter = encounterGenerator.Generate(EnvironmentConstants.Dungeon, partyLevel);
                }

                if (section == ContentsTypeConstants.Treasure)
                {
                    pool.Treasure = new DungeonTreasure();
                    pool.Treasure.Treasure = treasureGenerator.GenerateAtLevel(partyLevel);
                    pool.Treasure.Container = percentileSelector.SelectFrom(TableNameConstants.TreasureContainers);
                }

                if (section == ContentsConstants.MagicPool)
                {
                    pool.MagicPower = percentileSelector.SelectFrom(TableNameConstants.MagicPoolPowers);
                }
            }

            if (pool.MagicPower.Contains("ALIGNMENT"))
            {
                var alignment = percentileSelector.SelectFrom(TableNameConstants.MagicPoolAlignments);
                pool.MagicPower = pool.MagicPower.Replace("ALIGNMENT", alignment);
            }

            if (pool.MagicPower.Contains("DESTINATION"))
            {
                var destination = percentileSelector.SelectFrom(TableNameConstants.MagicPoolTeleportationDestinations);
                pool.MagicPower = pool.MagicPower.Replace("DESTINATION", destination);
            }

            return pool;
        }
    }
}
