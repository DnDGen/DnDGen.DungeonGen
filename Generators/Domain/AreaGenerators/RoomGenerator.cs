using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class RoomGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private AreaGenerator specialChamberGenerator;
        private ExitGenerator exitGenerator;
        private ContentsGenerator contentsGenerator;

        public RoomGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGenerator specialChamberGenerator, ExitGenerator exitGenerator, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.specialChamberGenerator = specialChamberGenerator;
            this.exitGenerator = exitGenerator;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var room = areaPercentileSelector.SelectFrom(TableNameConstants.Rooms);

            if (room.Type == AreaTypeConstants.Special)
                return specialChamberGenerator.Generate(level);

            var exits = exitGenerator.Generate(level, room.Length, room.Width);
            room.Contents = contentsGenerator.Generate(level);

            return new[] { room }.Union(exits);
        }
    }
}
