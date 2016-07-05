namespace DungeonGen.Domain.Generators.RuntimeFactories
{
    internal interface IAreaGeneratorFactory
    {
        bool HasSpecificGenerator(string areaType);
        AreaGenerator Build(string areaType);
    }
}
