using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class ChamberGenerator : AreaGenerator
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

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
        {
            var chamber = areaPercentileSelector.SelectFrom(TableNameConstants.Chambers);
            var chambers = new List<Area>();

            if (chamber.Type == AreaTypeConstants.Special)
            {
                var specialChambers = specialAreaGenerator.Generate(dungeonLevel, partyLevel, temperature);
                chambers.AddRange(specialChambers);
            }
            else
            {
                chambers.Add(chamber);
            }

            for (var i = chambers.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(chambers[i].Type))
                    chambers[i].Type = AreaTypeConstants.Chamber;

                var exits = exitGenerator.Generate(dungeonLevel, partyLevel, chambers[i].Length, chambers[i].Width, temperature);

                if (i + 1 == chambers.Count)
                    chambers.AddRange(exits);
                else
                    chambers.InsertRange(i + 1, exits);

                var newContents = contentsGenerator.Generate(partyLevel);
                chambers[i].Contents.Encounters = chambers[i].Contents.Encounters.Union(newContents.Encounters);
                chambers[i].Contents.Miscellaneous = chambers[i].Contents.Miscellaneous.Union(newContents.Miscellaneous);
                chambers[i].Contents.Traps = chambers[i].Contents.Traps.Union(newContents.Traps);
                chambers[i].Contents.Treasures = chambers[i].Contents.Treasures.Union(newContents.Treasures);
            }

            return chambers;
        }
    }
}
