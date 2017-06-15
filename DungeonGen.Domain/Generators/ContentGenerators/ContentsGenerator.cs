namespace DungeonGen.Domain.Generators.ContentGenerators
{
    internal interface ContentsGenerator
    {
        Contents Generate(int partyLevel);
    }
}
