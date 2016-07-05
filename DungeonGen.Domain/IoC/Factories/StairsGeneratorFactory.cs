using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using RollGen;

namespace DungeonGen.Domain.IoC.Factories
{
    internal static class StairsGeneratorFactory
    {

        public static AreaGenerator Build(IKernel kernel)
        {
            var areaPercentileSelector = kernel.Get<IAreaPercentileSelector>();
            var dice = kernel.Get<Dice>();
            var chamberGenerator = kernel.Get<AreaGenerator>(AreaTypeConstants.Chamber);

            return new StairsGenerator(areaPercentileSelector, dice, chamberGenerator);
        }
    }
}
