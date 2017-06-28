using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using TreasureGen.Generators;

namespace DungeonGen.Domain.Generators.ContentGenerators
{
    internal class DomainPoolGenerator : PoolGenerator
    {
        private readonly IPercentileSelector percentileSelector;
        private readonly JustInTimeFactory justInTimeFactory;

        public DomainPoolGenerator(IPercentileSelector percentileSelector, JustInTimeFactory justInTimeFactory)
        {
            this.percentileSelector = percentileSelector;
            this.justInTimeFactory = justInTimeFactory;
        }

        public Pool Generate(EncounterSpecifications environment)
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
                    var specifications = environment.Clone();
                    specifications.AllowAquatic = true;

                    var encounterGenerator = justInTimeFactory.Build<IEncounterGenerator>();
                    pool.Encounter = encounterGenerator.Generate(specifications);
                }

                if (section == ContentsTypeConstants.Treasure)
                {
                    pool.Treasure = new DungeonTreasure();
                    pool.Treasure.Container = percentileSelector.SelectFrom(TableNameConstants.TreasureContainers);

                    var treasureGenerator = justInTimeFactory.Build<ITreasureGenerator>();
                    pool.Treasure.Treasure = treasureGenerator.GenerateAtLevel(environment.Level);
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
