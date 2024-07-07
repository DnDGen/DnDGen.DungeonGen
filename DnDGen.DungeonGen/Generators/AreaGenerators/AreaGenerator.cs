using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using System.Collections.Generic;

namespace DnDGen.DungeonGen.Generators.AreaGenerators
{
    internal interface AreaGenerator
    {
        string AreaType { get; }
        IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment);
    }
}
