using EventGen;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class AreaGeneratorEventDecorator : AreaGenerator
    {
        public string AreaType
        {
            get { return innerGenerator.AreaType; }
        }

        private readonly AreaGenerator innerGenerator;
        private readonly GenEventQueue eventQueue;

        public AreaGeneratorEventDecorator(AreaGenerator innerGenerator, GenEventQueue eventQueue)
        {
            this.innerGenerator = innerGenerator;
            this.eventQueue = eventQueue;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating {temperature} {AreaType}s for level {partyLevel} party on dungeon level {dungeonLevel}");
            var areas = innerGenerator.Generate(dungeonLevel, partyLevel, temperature);

            var areaTypes = areas.Select(a => a.Type);
            eventQueue.Enqueue("DungeonGen", $"Finished generating {areas.Count()} {AreaType}s: [{string.Join(", ", areaTypes)}]");

            return areas;
        }
    }
}
