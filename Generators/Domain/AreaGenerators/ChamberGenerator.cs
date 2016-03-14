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

        public ChamberGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGenerator specialChamberGenerator, ExitGenerator exitGenerator, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.specialAreaGenerator = specialChamberGenerator;
            this.exitGenerator = exitGenerator;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var chamber = areaPercentileSelector.SelectFrom(TableNameConstants.Chambers);

            if (chamber.Type == AreaTypeConstants.Special)
                chamber = specialAreaGenerator.Generate(level).Single();

            var exits = exitGenerator.Generate(level, chamber.Length, chamber.Width);
            chamber.Contents = contentsGenerator.Generate(level);

            return new[] { chamber }.Union(exits);
        }
    }
}
