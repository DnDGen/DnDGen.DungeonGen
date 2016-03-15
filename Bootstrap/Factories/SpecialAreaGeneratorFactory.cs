using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class SpecialAreaGeneratorFactory
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
