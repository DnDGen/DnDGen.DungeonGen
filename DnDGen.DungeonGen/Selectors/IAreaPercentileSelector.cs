using DnDGen.DungeonGen.Models;

namespace DnDGen.DungeonGen.Selectors
{
    internal interface IAreaPercentileSelector
    {
        Area SelectFrom(string tableName);
    }
}
