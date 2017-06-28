using EncounterGen.Generators;
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

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            eventQueue.Enqueue("DungeonGen", $"Generating {environment.Description} {AreaType} on dungeon level {dungeonLevel}");
            var areas = innerGenerator.Generate(dungeonLevel, environment);

            var areaTypes = areas.Select(a => a.Type);
            var message = $"Generated {areas.Count()} areas for {AreaType}";

            if (areaTypes.Any(t => !string.IsNullOrEmpty(t)))
                message += $": [{string.Join(", ", areaTypes)}]";

            eventQueue.Enqueue("DungeonGen", message);

            return areas;
        }
    }
}
