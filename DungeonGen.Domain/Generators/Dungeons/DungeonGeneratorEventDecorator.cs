using EventGen;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.Dungeons
{
    internal class DungeonGeneratorEventDecorator : IDungeonGenerator
    {
        private readonly IDungeonGenerator innerGenerator;
        private readonly GenEventQueue eventQueue;

        public DungeonGeneratorEventDecorator(IDungeonGenerator innerGenerator, GenEventQueue eventQueue)
        {
            this.innerGenerator = innerGenerator;
            this.eventQueue = eventQueue;
        }

        public IEnumerable<Area> GenerateFromDoor(int dungeonLevel, int partyLevel, string temperature)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating {temperature} dungeon area from door for level {partyLevel} party on dungeon level {dungeonLevel}");
            var areas = innerGenerator.GenerateFromDoor(dungeonLevel, partyLevel, temperature);

            var areaTypes = areas.Select(a => a.Type);
            eventQueue.Enqueue("DungeonGen", $"Finished generating {areas.Count()} areas: [{string.Join(", ", areaTypes)}]");

            return areas;
        }

        public IEnumerable<Area> GenerateFromHall(int dungeonLevel, int partyLevel, string temperature)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating {temperature} dungeon area from hall for level {partyLevel} party on dungeon level {dungeonLevel}");
            var areas = innerGenerator.GenerateFromHall(dungeonLevel, partyLevel, temperature);

            var areaTypes = areas.Select(a => a.Type);
            eventQueue.Enqueue("DungeonGen", $"Finished generating {areas.Count()} areas: [{string.Join(", ", areaTypes)}]");

            return areas;
        }
    }
}
