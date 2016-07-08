using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class ParallelPassageGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var hallGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Hall);

            return new ParallelPassageGenerator(hallGenerator);
        }
    }
}
