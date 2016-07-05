namespace DungeonGen.Domain.Selectors
{
    internal interface IAreaPercentileSelector
    {
        Area SelectFrom(string tableName);
    }
}
