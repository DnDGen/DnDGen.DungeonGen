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
        private AreaGenerator specialAreaGenerator;
        private ExitGenerator exitGenerator;
        private ContentsGenerator contentsGenerator;

        public RoomGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGenerator specialAreaGenerator, ExitGenerator exitGenerator, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.specialAreaGenerator = specialAreaGenerator;
            this.exitGenerator = exitGenerator;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var room = areaPercentileSelector.SelectFrom(TableNameConstants.Rooms);
            var rooms = new List<Area>();

            if (room.Type == AreaTypeConstants.Special)
            {
                var specialChambers = specialAreaGenerator.Generate(dungeonLevel, partyLevel);
                rooms.AddRange(specialChambers);
            }
            else
            {
                rooms.Add(room);
            }

            for (var i = rooms.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(rooms[i].Type))
                    rooms[i].Type = AreaTypeConstants.Room;

                var exits = exitGenerator.Generate(dungeonLevel, partyLevel, rooms[i].Length, rooms[i].Width);

                if (i + 1 == rooms.Count)
                    rooms.AddRange(exits);
                else
                    rooms.InsertRange(i + 1, exits);

                var newContents = contentsGenerator.Generate(partyLevel);
                rooms[i].Contents.Encounters = rooms[i].Contents.Encounters.Union(newContents.Encounters);
                rooms[i].Contents.Miscellaneous = rooms[i].Contents.Miscellaneous.Union(newContents.Miscellaneous);
                rooms[i].Contents.Traps = rooms[i].Contents.Traps.Union(newContents.Traps);
                rooms[i].Contents.Treasures = rooms[i].Contents.Treasures.Union(newContents.Treasures);
            }

            return rooms;
        }
    }
}
