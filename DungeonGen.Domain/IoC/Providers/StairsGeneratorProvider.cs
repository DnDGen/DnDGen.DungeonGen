using DungeonGen.Domain.Generators;
using DungeonGen.Domain.Generators.AreaGenerators;
using DungeonGen.Domain.Selectors;
using Ninject;
using Ninject.Activation;
using RollGen;

namespace DungeonGen.Domain.IoC.Providers
{
    class StairsGeneratorProvider : Provider<AreaGenerator>
    {
        protected override AreaGenerator CreateInstance(IContext context)
        {
            var areaPercentileSelector = context.Kernel.Get<IAreaPercentileSelector>();
            var dice = context.Kernel.Get<Dice>();
            var chamberGenerator = context.Kernel.Get<AreaGenerator>(AreaTypeConstants.Chamber);

            return new StairsGenerator(areaPercentileSelector, dice, chamberGenerator);
        }
    }
}
