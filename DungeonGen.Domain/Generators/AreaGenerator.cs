using System.Collections.Generic;

namespace DungeonGen.Domain.Generators
{
    internal interface AreaGenerator
    {
        IEnumerable<Area> Generate(int dungeonLevel, int partyLevel);
    }
}
