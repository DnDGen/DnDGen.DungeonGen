using EncounterGen.Generators;
using System.Collections.Generic;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal interface AreaGenerator
    {
        string AreaType { get; }
        IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment);
    }
}
