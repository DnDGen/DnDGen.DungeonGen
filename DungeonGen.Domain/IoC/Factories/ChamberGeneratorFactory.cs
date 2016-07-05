using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class ChamberGeneratorFactory
    {
        public static AreaGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var specialAreaGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Special);
            var exitGenerator = kernel.Get<ExitGenerator>(AreaTypeConstants.Chamber);
            var contentsGenerator = kernel.Get<ContentsGenerator>();

            return new ChamberGenerator(areaPercentileSelector, specialAreaGenerator, exitGenerator, contentsGenerator);
        }
    }
}
