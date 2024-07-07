using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;

namespace DnDGen.DungeonGen.Generators
{
    internal interface PoolGenerator
    {
        Pool Generate(EncounterSpecifications environment);
    }
}
