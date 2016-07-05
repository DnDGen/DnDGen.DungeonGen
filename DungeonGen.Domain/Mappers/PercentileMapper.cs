using System.Collections.Generic;

namespace DungeonGen.Domain.Mappers
{
    internal interface PercentileMapper
    {
        Dictionary<int, string> Map(string tableName);
    }
}
