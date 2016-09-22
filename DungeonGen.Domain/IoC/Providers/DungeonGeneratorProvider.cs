using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.RuntimeFactories;
using DungeonGen.Domain.Selectors;
using EncounterGen.Generators;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class DungeonGeneratorProvider : Provider<IDungeonGenerator>
    {
        protected override IDungeonGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var areaGeneratorFactory = context.Kernel.Get<IAreaGeneratorFactory>();
            var encounterGenerator = context.Kernel.Get<IEncounterGenerator>();
            var trapGenerator = context.Kernel.Get<ITrapGenerator>();
            var percentileSelector = context.Kernel.Get<IPercentileSelector>();
            var hallGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new DungeonGenerator(areaPercentileSelector, areaGeneratorFactory, encounterGenerator, trapGenerator, percentileSelector, hallGenerator);
        }
    }
}
