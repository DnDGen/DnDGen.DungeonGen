using EncounterGen.Generators;
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

        public IEnumerable<Area> GenerateFromDoor(int dungeonLevel, EncounterSpecifications environment)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating dungeon area in {environment.Description} from door on dungeon level {dungeonLevel}");
            var areas = innerGenerator.GenerateFromDoor(dungeonLevel, environment);

            var areaTypes = areas.Select(a => a.Type);
            eventQueue.Enqueue("DungeonGen", $"Generated {areas.Count()} areas: [{string.Join(", ", areaTypes)}]");

            return areas;
        }

        public IEnumerable<Area> GenerateFromHall(int dungeonLevel, EncounterSpecifications environment)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating dungeon area in {environment.Description} from hall on dungeon level {dungeonLevel}");
            var areas = innerGenerator.GenerateFromHall(dungeonLevel, environment);

            var areaTypes = areas.Select(a => a.Type);
            eventQueue.Enqueue("DungeonGen", $"Generated {areas.Count()} areas: [{string.Join(", ", areaTypes)}]");

            return areas;
        }
    }
}
