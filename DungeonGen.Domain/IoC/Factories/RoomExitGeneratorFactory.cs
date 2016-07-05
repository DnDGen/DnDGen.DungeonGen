using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class RoomExitGeneratorFactory
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
