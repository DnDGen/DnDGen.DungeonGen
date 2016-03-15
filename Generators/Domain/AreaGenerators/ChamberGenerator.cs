using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class ChamberGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private AreaGenerator specialAreaGenerator;
        private ExitGenerator exitGenerator;
        private ContentsGenerator contentsGenerator;

        public ChamberGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGenerator specialAreaGenerator, ExitGenerator exitGenerator, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.specialAreaGenerator = specialAreaGenerator;
            this.exitGenerator = exitGenerator;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var chamber = areaPercentileSelector.SelectFrom(TableNameConstants.Chambers);
            var chambers = new List<Area>();

            if (chamber.Type == AreaTypeConstants.Special)
            {
                var specialChambers = specialAreaGenerator.Generate(level);
                chambers.AddRange(specialChambers);
            }
            else
            {
                chambers.Add(chamber);
            }

            for (var i = chambers.Count - 1; i >= 0; i--)
            {
                var exits = exitGenerator.Generate(level, chambers[i].Length, chambers[i].Width);

                if (i + 1 == chambers.Count)
                    chambers.AddRange(exits);
                else
                    chambers.InsertRange(i + 1, exits);

                var newContents = contentsGenerator.Generate(level);
                chambers[i].Contents.Encounters = chambers[i].Contents.Encounters.Union(newContents.Encounters);
                chambers[i].Contents.Miscellaneous = chambers[i].Contents.Miscellaneous.Union(newContents.Miscellaneous);
                chambers[i].Contents.Traps = chambers[i].Contents.Traps.Union(newContents.Traps);
                chambers[i].Contents.Treasures = chambers[i].Contents.Treasures.Union(newContents.Treasures);
            }

            return chambers;
        }
    }
}
