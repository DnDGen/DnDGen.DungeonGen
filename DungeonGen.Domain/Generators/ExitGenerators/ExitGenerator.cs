using EncounterGen.Generators;
using System.Collections.Generic;

namespace DungeonGen.Domain.Generators.ExitGenerators
{
    internal interface ExitGenerator
    {
        IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment, int length, int width);
    }
}
