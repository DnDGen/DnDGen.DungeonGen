using DungeonGen.Domain.Generators.AreaGenerators;

namespace DungeonGen.Domain.Generators.Factories
{
    internal interface AreaGeneratorFactory
    {
        bool HasSpecificGenerator(string areaType);
        AreaGenerator Build(string areaType);
    }
}
