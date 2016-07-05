using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.RuntimeFactories;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class AreaGeneratorFactoryFactory
    {
        public static IAreaGeneratorFactory Build(IKernel kernel)
        {
            var chamberGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Chamber);
            var doorGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Door);
            var roomGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Room);
            var sidePassageGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.SidePassage);
            var stairsGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Stairs);
            var turnGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Turn);

            return new AreaGeneratorFactory(chamberGenerator, doorGenerator, roomGenerator, sidePassageGenerator, stairsGenerator, turnGenerator);
        }
    }
}
