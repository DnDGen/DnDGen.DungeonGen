using EncounterGen.Generators;
using System.Collections.Generic;

namespace DungeonGen
{
    public interface IDungeonGenerator
    {
        IEnumerable<Area> GenerateFromHall(int dungeonLevel, EncounterSpecifications environment);
        IEnumerable<Area> GenerateFromDoor(int dungeonLevel, EncounterSpecifications environment);
    }
}
