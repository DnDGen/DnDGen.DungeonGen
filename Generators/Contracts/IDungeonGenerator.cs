using DungeonGen.Common;

namespace DungeonGen.Generators
{
    public interface IDungeonGenerator
    {
        Area GenerateFromHall(int level);
        Area GenerateFromDoor(int level);
    }
}
