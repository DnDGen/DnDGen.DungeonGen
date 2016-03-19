using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.RuntimeFactories;
using DungeonGen.Generators.Domain.RuntimeFactories.Domain;
using Ninject;

namespace DungeonGen.Bootstrap.Factories
{
    public static class AreaGeneratorFactoryFactory
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
