using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using System.Collections.Generic;

namespace DnDGen.DungeonGen.Generators.ExitGenerators
{
    internal interface ExitGenerator
    {
        IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment, int length, int width);
    }
}
