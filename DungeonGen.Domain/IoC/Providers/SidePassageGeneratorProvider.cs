using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class SidePassageGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var percentileSelector = context.Kernel.Get<IPercentileSelector>();
            var hallGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new SidePassageGenerator(percentileSelector, hallGenerator);
        }
    }
}
