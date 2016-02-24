using DungeonGen.Common;
using System.Collections.Generic;

namespace DungeonGen.Generators
{
    public interface IDungeonGenerator
    {
        IEnumerable<Area> GenerateFromHall(int level);
        IEnumerable<Area> GenerateFromDoor(int level);
    }
}
