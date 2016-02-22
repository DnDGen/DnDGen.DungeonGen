using System.Collections.Generic;

namespace DungeonGen.Mappers
{
    public interface PercentileMapper
    {
        Dictionary<int, string> Map(string tableName);
    }
}
