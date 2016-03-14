using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class RoomGeneratorFactory
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
