using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class SidePassageGeneratorFactory
    {
        public static AreaGenerator Build(IKernel kernel)
        {
            var percentileSelector = kernel.Get<IPercentileSelector>();
            var hallGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new SidePassageGenerator(percentileSelector, hallGenerator);
        }
    }
}
