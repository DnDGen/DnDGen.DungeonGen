namespace DungeonGen.Domain.Generators
{
    internal interface PoolGenerator
    {
        Pool Generate(int partyLevel, string temperature);
    }
}
