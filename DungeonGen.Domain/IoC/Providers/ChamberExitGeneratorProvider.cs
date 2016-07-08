using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.ExitGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class ChamberExitGeneratorProvider : Provider<ExitGenerator>
    {
        protected override ExitGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var hallGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);
            var doorGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Door);
            var percentileSelector = context.Kernel.Get<IPercentileSelector>();

            return new ChamberExitGenerator(areaPercentileSelector, hallGenerator, doorGenerator, percentileSelector);
        }
    }
}
