using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Generators.Domain.RuntimeFactories.Domain;
using DungeonGen.Selectors;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class AreaGeneratorFactoryFactory
    {
        public static IAreaGeneratorFactory Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var specialAreaGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Special);
            var chamberExitGenerator = kernel.Get<ExitGenerator>(AreaTypeConstants.Chamber);
            var contentsGenerator = kernel.Get<ContentsGenerator>();
            var percentileSelector = kernel.Get<IPercentileSelector>();
            var hallGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new AreaGeneratorFactory(areaPercentileSelector, specialAreaGenerator, chamberExitGenerator, contentsGenerator,
                hallGenerator, percentileSelector);
        }
    }
}
