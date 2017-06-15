using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;
using TreasureGen.Generators;

namespace DungeonGen.Domain.Generators.ContentGenerators
{
    internal class DomainContentsGenerator : ContentsGenerator
    {
        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly IPercentileSelector percentileSelector;
        private readonly JustInTimeFactory justInTimeFactory;

        public DomainContentsGenerator(IAreaPercentileSelector areaPercentileSelector, IPercentileSelector percentileSelector, JustInTimeFactory justInTimeFactory)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.percentileSelector = percentileSelector;
            this.justInTimeFactory = justInTimeFactory;
        }

        public Contents Generate(int partyLevel)
        {
            var areaContents = areaPercentileSelector.SelectFrom(TableNameConstants.Contents);

            var miscellaneousContents = new List<string>();
            miscellaneousContents.AddRange(areaContents.Contents.Miscellaneous);

            var minorFeatures = GetFeatures(areaContents.Length, TableNameConstants.MinorFeatures);
            miscellaneousContents.AddRange(minorFeatures);

            var majorFeatures = GetFeatures(areaContents.Width, TableNameConstants.MajorFeatures);
            miscellaneousContents.AddRange(majorFeatures);

            areaContents.Contents.Miscellaneous = miscellaneousContents;

            if (areaContents.Contents.Miscellaneous.Contains(ContentsTypeConstants.Treasure))
            {
                var dungeonTreasure = new DungeonTreasure();
                dungeonTreasure.Concealment = percentileSelector.SelectFrom(TableNameConstants.TreasureConcealment);
                dungeonTreasure.Container = percentileSelector.SelectFrom(TableNameConstants.TreasureContainers);

                var treasureGenerator = justInTimeFactory.Build<ITreasureGenerator>();
                dungeonTreasure.Treasure = treasureGenerator.GenerateAtLevel(partyLevel);

                areaContents.Contents.Treasures = new[] { dungeonTreasure };
            }

            return areaContents.Contents;
        }

        private IEnumerable<string> GetFeatures(int quantity, string tableName)
        {
            var features = new List<string>();

            while (quantity-- > 0)
            {
                var feature = percentileSelector.SelectFrom(tableName);
                features.Add(feature);
            }

            return features;
        }
    }
}
