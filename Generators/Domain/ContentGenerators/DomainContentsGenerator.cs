using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;
using System.Linq;
using TreasureGen.Generators;

namespace DungeonGen.Generators.Domain.ContentGenerators
{
    public class DomainContentsGenerator : ContentsGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private IPercentileSelector percentileSelector;
        private ITreasureGenerator treasureGenerator;

        public DomainContentsGenerator(IAreaPercentileSelector areaPercentileSelector, IPercentileSelector percentileSelector, ITreasureGenerator treasureGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.percentileSelector = percentileSelector;
            this.treasureGenerator = treasureGenerator;
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
                dungeonTreasure.Treasure = treasureGenerator.GenerateAtLevel(partyLevel);
                dungeonTreasure.Concealment = percentileSelector.SelectFrom(TableNameConstants.TreasureConcealment);
                dungeonTreasure.Container = percentileSelector.SelectFrom(TableNameConstants.TreasureContainers);

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
