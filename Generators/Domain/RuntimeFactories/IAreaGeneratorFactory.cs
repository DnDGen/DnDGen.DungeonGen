namespace DungeonGen.Generators.Domain.RuntimeFactories
{
    public interface IAreaGeneratorFactory
    {
        bool HasSpecificGenerator(string areaType);
        AreaGenerator Build(string areaType);
    }
}
