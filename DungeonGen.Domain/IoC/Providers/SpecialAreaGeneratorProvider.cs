using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class SpecialAreaGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var percentileSelector = context.Kernel.Get<IPercentileSelector>();
            var poolGenerator = context.Kernel.Get<PoolGenerator>();
            var caveGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Cave);

            return new SpecialAreaGenerator(areaPercentileSelector, percentileSelector, poolGenerator, caveGenerator);
        }
    }
}
