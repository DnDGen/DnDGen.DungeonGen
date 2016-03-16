using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class HallGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private IPercentileSelector percentileSelector;

        public HallGenerator(IAreaPercentileSelector areaPercentileSelector, IPercentileSelector percentileSelector)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.percentileSelector = percentileSelector;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var hall = areaPercentileSelector.SelectFrom(TableNameConstants.Halls);

            if (hall.Type != AreaTypeConstants.Special)
                return new[] { hall };

            var tableName = string.Format(TableNameConstants.SpecialAREA, AreaTypeConstants.Hall);
            hall = areaPercentileSelector.SelectFrom(tableName);

            if (hall.Contents.Miscellaneous.Contains(ContentsConstants.Chasm))
            {
                hall = AddContents(hall, TableNameConstants.ChasmCrossing);
            }

            if (hall.Contents.Miscellaneous.Contains(ContentsConstants.River))
            {
                hall = AddContents(hall, TableNameConstants.RiverCrossing);
            }

            if (hall.Contents.Miscellaneous.Contains(ContentsConstants.Stream))
            {
                hall = AddContents(hall, TableNameConstants.StreamCrossing);
            }

            if (hall.Contents.Miscellaneous.Contains(ContentsConstants.Gallery))
            {
                hall = AddContents(hall, TableNameConstants.GalleryStairs);
            }

            if (hall.Contents.Miscellaneous.Contains(ContentsConstants.GalleryStairs_Beginning))
            {
                hall = AddContents(hall, TableNameConstants.AdditionalGalleryStairs);
            }

            return new[] { hall };
        }

        private Area AddContents(Area area, string tableName)
        {
            var additionalContents = percentileSelector.SelectFrom(tableName);

            if (string.IsNullOrEmpty(additionalContents))
                return area;

            area.Contents.Miscellaneous = area.Contents.Miscellaneous.Union(new[] { additionalContents });

            return area;
        }
    }
}
