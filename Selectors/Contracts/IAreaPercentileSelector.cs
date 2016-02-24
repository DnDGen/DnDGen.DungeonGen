using DungeonGen.Common;

namespace DungeonGen.Selectors
{
    public interface IAreaPercentileSelector
    {
        Area SelectFrom(string tableName);
    }
}
