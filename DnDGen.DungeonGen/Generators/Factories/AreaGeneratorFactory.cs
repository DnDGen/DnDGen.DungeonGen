using DnDGen.DungeonGen.Generators.AreaGenerators;

namespace DnDGen.DungeonGen.Generators.Factories
{
    internal interface AreaGeneratorFactory
    {
        bool HasSpecificGenerator(string areaType);
        AreaGenerator Build(string areaType);
    }
}
