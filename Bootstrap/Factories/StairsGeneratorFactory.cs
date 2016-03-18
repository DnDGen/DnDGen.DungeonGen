using DungeonGen.Common;
using DungeonGen.Generators;
using DungeonGen.Generators.Domain.AreaGenerators;
using DungeonGen.Selectors;
using Ninject;
using RollGen;

namespace DungeonGen.Bootstrap.Factories
{
    public static class StairsGeneratorFactory
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
