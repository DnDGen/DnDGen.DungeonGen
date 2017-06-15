using System.Collections.Generic;

namespace DungeonGen.Domain.Generators.ExitGenerators
{
    internal interface ExitGenerator
    {
        IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, int length, int width, string temperature);
    }
}
