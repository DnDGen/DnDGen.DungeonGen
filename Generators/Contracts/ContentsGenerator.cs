using DungeonGen.Common;

namespace DungeonGen.Generators
{
    public interface ContentsGenerator
    {
        Contents Generate(int level);
    }
}
