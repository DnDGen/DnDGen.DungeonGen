using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;

namespace DungeonGen.Domain.IoC.Providers
{
    class ChamberGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var specialAreaGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Special);
            var exitGenerator = context.Kernel.Get<ExitGenerator>(AreaTypeConstants.Chamber);
            var contentsGenerator = context.Kernel.Get<ContentsGenerator>();

            return new ChamberGenerator(areaPercentileSelector, specialAreaGenerator, exitGenerator, contentsGenerator);
        }
    }
}
