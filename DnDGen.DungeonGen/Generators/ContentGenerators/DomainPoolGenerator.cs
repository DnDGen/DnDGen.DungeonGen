using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Generators;
using DnDGen.Infrastructure.Selectors.Percentiles;
using DnDGen.TreasureGen.Generators;

namespace DnDGen.DungeonGen.Generators.ContentGenerators
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
            var selectedPool = percentileSelector.SelectFrom(Config.Name, TableNameConstants.Pools);

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
                    pool.Treasure.Container = percentileSelector.SelectFrom(Config.Name, TableNameConstants.TreasureContainers);

                    var treasureGenerator = justInTimeFactory.Build<ITreasureGenerator>();
                    pool.Treasure.Treasure = treasureGenerator.GenerateAtLevel(environment.Level);
                }

                if (section == ContentsConstants.MagicPool)
                {
                    pool.MagicPower = percentileSelector.SelectFrom(Config.Name, TableNameConstants.MagicPoolPowers);
                }
            }

            if (pool.MagicPower.Contains("ALIGNMENT"))
            {
                var alignment = percentileSelector.SelectFrom(Config.Name, TableNameConstants.MagicPoolAlignments);
                pool.MagicPower = pool.MagicPower.Replace("ALIGNMENT", alignment);
            }

            if (pool.MagicPower.Contains("DESTINATION"))
            {
                var destination = percentileSelector.SelectFrom(Config.Name, TableNameConstants.MagicPoolTeleportationDestinations);
                pool.MagicPower = pool.MagicPower.Replace("DESTINATION", destination);
            }

            return pool;
        }
    }
}
