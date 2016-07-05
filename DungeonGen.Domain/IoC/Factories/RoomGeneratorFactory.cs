using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class RoomGeneratorFactory
    {
        public static AreaGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var specialAreaGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Special);
            var exitGenerator = kernel.Get<ExitGenerator>(AreaTypeConstants.Room);
            var contentsGenerator = kernel.Get<ContentsGenerator>();

            return new RoomGenerator(areaPercentileSelector, specialAreaGenerator, exitGenerator, contentsGenerator);
        }
    }
}
