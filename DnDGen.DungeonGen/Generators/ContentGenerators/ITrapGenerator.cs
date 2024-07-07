using DnDGen.DungeonGen.Models;

namespace DnDGen.DungeonGen.Generators.ContentGenerators
{
    internal interface ITrapGenerator
    {
        Trap Generate(int partyLevel);
    }
}
