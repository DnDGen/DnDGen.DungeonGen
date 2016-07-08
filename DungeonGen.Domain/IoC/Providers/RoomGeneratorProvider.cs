using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class RoomGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var specialAreaGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Special);
            var exitGenerator = context.Kernel.Get<ExitGenerator>(AreaTypeConstants.Room);
            var contentsGenerator = context.Kernel.Get<ContentsGenerator>();

            return new RoomGenerator(areaPercentileSelector, specialAreaGenerator, exitGenerator, contentsGenerator);
        }
    }
}
