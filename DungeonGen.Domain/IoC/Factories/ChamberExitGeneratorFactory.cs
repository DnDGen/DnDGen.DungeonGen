using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class ChamberExitGeneratorFactory
    {
        public static ExitGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var hallGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);
            var doorGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Door);
            var percentileSelector = kernel.Get<IPercentileSelector>();

            return new ChamberExitGenerator(areaPercentileSelector, hallGenerator, doorGenerator, percentileSelector);
        }
    }
}
