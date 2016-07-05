using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class SpecialAreaGeneratorFactory
    {
        public static AreaGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var percentileSelector = kernel.Get<IPercentileSelector>();
            var poolGenerator = kernel.Get<PoolGenerator>();
            var caveGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Cave);

            return new SpecialAreaGenerator(areaPercentileSelector, percentileSelector, poolGenerator, caveGenerator);
        }
    }
}
