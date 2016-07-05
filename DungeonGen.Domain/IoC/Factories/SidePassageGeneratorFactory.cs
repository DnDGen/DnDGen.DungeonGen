using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class SidePassageGeneratorFactory
    {
        public static AreaGenerator Build(IKernel kernel)
        {
            var percentileSelector = kernel.Get<IPercentileSelector>();
            var hallGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new SidePassageGenerator(percentileSelector, hallGenerator);
        }
    }
}
