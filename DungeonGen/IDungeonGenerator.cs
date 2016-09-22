using System.Collections.Generic;

namespace DungeonGen
{
    public interface IDungeonGenerator
    {
        IEnumerable<Area> GenerateFromHall(int dungeonLevel, int partyLevel, string temperature);
        IEnumerable<Area> GenerateFromDoor(int dungeonLevel, int partyLevel, string temperature);
    }
}
