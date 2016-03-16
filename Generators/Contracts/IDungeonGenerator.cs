using DungeonGen.Common;
using System.Collections.Generic;

namespace DungeonGen.Generators
{
    public interface IDungeonGenerator
    {
        IEnumerable<Area> GenerateFromHall(int dungeonLevel, int partyLevel);
        IEnumerable<Area> GenerateFromDoor(int dungeonLevel, int partyLevel);
    }
}
