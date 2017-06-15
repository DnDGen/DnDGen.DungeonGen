using DungeonGen.Domain.Generators.Factories;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class JustInTimeFactoryProvider : Provider<JustInTimeFactory>
    {
        protected override JustInTimeFactory CreateInstance(IContext context)
        {
            return new NinjectJustInTimeFactory(context.Kernel);
        }
    }
}
