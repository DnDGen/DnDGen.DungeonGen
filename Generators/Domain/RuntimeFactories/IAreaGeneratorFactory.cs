namespace DungeonGen.Generators.Domain.RuntimeFactories
{
    public interface IAreaGeneratorFactory
    {
        bool HasSpecificGenerator(string areaType);
        IAreaGenerator Build(string areaType);
    }
}
