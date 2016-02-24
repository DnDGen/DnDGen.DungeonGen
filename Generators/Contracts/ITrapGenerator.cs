using DungeonGen.Common;

namespace DungeonGen.Generators
{
    public interface ITrapGenerator
    {
        Trap Generate(int level);
    }
}
