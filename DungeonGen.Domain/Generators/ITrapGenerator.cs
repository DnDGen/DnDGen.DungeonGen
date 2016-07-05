namespace DungeonGen.Domain.Generators
{
    internal interface ITrapGenerator
    {
        Trap Generate(int partyLevel);
    }
}
