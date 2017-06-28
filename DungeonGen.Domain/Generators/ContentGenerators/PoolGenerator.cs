using EncounterGen.Generators;

namespace DungeonGen.Domain.Generators
{
    internal interface PoolGenerator
    {
        Pool Generate(EncounterSpecifications environment);
    }
}
