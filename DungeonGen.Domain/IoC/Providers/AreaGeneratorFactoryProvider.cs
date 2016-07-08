using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.RuntimeFactories;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class AreaGeneratorFactoryProvider : Provider<IAreaGeneratorFactory>
    {
        protected override IAreaGeneratorFactory CreateInstance(IContext context)
        {
            var chamberGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Chamber);
            var doorGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Door);
            var roomGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Room);
            var sidePassageGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.SidePassage);
            var stairsGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Stairs);
            var turnGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Turn);

            return new AreaGeneratorFactory(chamberGenerator, doorGenerator, roomGenerator, sidePassageGenerator, stairsGenerator, turnGenerator);
        }
    }
}
