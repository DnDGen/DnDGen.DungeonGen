using DnDGen.DungeonGen.Generators.ContentGenerators;
using DnDGen.DungeonGen.Generators.ExitGenerators;
using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.DungeonGen.Selectors;
using DnDGen.DungeonGen.Tables;
using DnDGen.EncounterGen.Generators;
using DnDGen.Infrastructure.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.AreaGenerators
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

        public RoomGenerator(
            IAreaPercentileSelector areaPercentileSelector,
            AreaGeneratorFactory areaGeneratorFactory,
            JustInTimeFactory justInTimeFactory,
            ContentsGenerator contentsGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
            this.justInTimeFactory = justInTimeFactory;
            this.contentsGenerator = contentsGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var room = areaPercentileSelector.SelectFrom(TableNameConstants.Rooms);
            var rooms = new List<Area>();

            if (room.Type == AreaTypeConstants.Special)
            {
                var specialAreaGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Special);
                var specialAreas = specialAreaGenerator.Generate(dungeonLevel, environment);
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
                var exits = exitGenerator.Generate(dungeonLevel, environment, rooms[i].Length, rooms[i].Width);

                if (i + 1 == rooms.Count)
                    rooms.AddRange(exits);
                else
                    rooms.InsertRange(i + 1, exits);

                var newContents = contentsGenerator.Generate(environment.Level);
                rooms[i].Contents.Encounters = rooms[i].Contents.Encounters.Union(newContents.Encounters);
                rooms[i].Contents.Miscellaneous = rooms[i].Contents.Miscellaneous.Union(newContents.Miscellaneous);
                rooms[i].Contents.Traps = rooms[i].Contents.Traps.Union(newContents.Traps);
                rooms[i].Contents.Treasures = rooms[i].Contents.Treasures.Union(newContents.Treasures);
            }

            return rooms;
        }
    }
}
