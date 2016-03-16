using DungeonGen.Common;

namespace DungeonGen.Generators
{
    public interface PoolGenerator
    {
        Pool Generate(int partyLevel);
    }
}
