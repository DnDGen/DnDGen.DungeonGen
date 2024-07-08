using DnDGen.DungeonGen.Models;

namespace DnDGen.DungeonGen.Generators.ContentGenerators
{
    internal interface ContentsGenerator
    {
        Contents Generate(int partyLevel);
    }
}
