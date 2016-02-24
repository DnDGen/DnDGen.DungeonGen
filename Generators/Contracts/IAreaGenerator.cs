using DungeonGen.Common;
using System.Collections.Generic;

namespace DungeonGen.Generators
{
    public interface IAreaGenerator
    {
        IEnumerable<Area> Generate(int level);
    }
}
