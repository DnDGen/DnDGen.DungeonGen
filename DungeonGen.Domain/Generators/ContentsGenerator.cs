namespace DungeonGen.Domain.Generators
{
    internal interface ContentsGenerator
    {
        Contents Generate(int partyLevel);
    }
}
