using DungeonGen.Common;
using System.Collections.Generic;

namespace DungeonGen.Generators
{
    public interface ExitGenerator
    {
        IEnumerable<Area> Generate(int level, int length, int width);
    }
}
