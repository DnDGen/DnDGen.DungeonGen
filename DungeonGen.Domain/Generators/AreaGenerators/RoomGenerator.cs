using DungeonGen.Domain.Generators.ContentGenerators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class RoomGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Room; }
        }

        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly JustInTimeFactory justInTimeFactory;
        private readonly ContentsGenerator contentsGenerator;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public RoomGenerator(IAreaPercentileSelector areaPercentileSelector, AreaGeneratorFactory areaGeneratorFactory, JustInTimeFactory justInTimeFactory, ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.justInTimeFactory = justInTimeFactory;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
        {
            var room = areaPercentileSelector.SelectFrom(TableNameConstants.Rooms);
            var rooms = new List<Area>();

            if (room.Type == AreaTypeConstants.Special)
            {
                var specialAreaGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Special);
                var specialAreas = specialAreaGenerator.Generate(dungeonLevel, partyLevel, temperature);
                rooms.AddRange(specialAreas);
            }
            else
            {
                rooms.Add(room);
            }

            for (var i = rooms.Count - 1; i >= 0; i--)
            {
                //INFO: This is for special rooms
                if (string.IsNullOrEmpty(rooms[i].Type))
                    rooms[i].Type = AreaTypeConstants.Room;

                var exitGenerator = justInTimeFactory.Build<ExitGenerator>(AreaTypeConstants.Room);
                var exits = exitGenerator.Generate(dungeonLevel, partyLevel, rooms[i].Length, rooms[i].Width, temperature);

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
