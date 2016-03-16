using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.ExitGenerators;
using DungeonGen.Selectors;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class RoomExitGeneratorFactory
    {
        public static ExitGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var hallGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);
            var doorGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Door);
            var percentileSelector = kernel.Get<IPercentileSelector>();

            return new RoomExitGenerator(areaPercentileSelector, hallGenerator, doorGenerator, percentileSelector);
        }
    }
}
