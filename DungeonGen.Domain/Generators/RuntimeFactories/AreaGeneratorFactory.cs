using System.Collections.Generic;

namespace DungeonGen.Domain.Generators.RuntimeFactories
{
    internal class AreaGeneratorFactory : IAreaGeneratorFactory
    {
        private readonly Dictionary<string, AreaGenerator> areaGenerators;

        public AreaGeneratorFactory(AreaGenerator chamberGenerator, AreaGenerator doorGenerator, AreaGenerator roomGenerator, AreaGenerator sidePassageGenerator, AreaGenerator stairsGenerator, AreaGenerator turnGenerator, AreaGenerator parallelPassageGenerator)
        {
            areaGenerators = new Dictionary<string, AreaGenerator>();
            areaGenerators[AreaTypeConstants.Chamber] = chamberGenerator;
            areaGenerators[AreaTypeConstants.Door] = doorGenerator;
            areaGenerators[AreaTypeConstants.Room] = roomGenerator;
            areaGenerators[AreaTypeConstants.SidePassage] = sidePassageGenerator;
            areaGenerators[AreaTypeConstants.Stairs] = stairsGenerator;
            areaGenerators[AreaTypeConstants.Turn] = turnGenerator;
            areaGenerators[SidePassageConstants.ParallelPassage] = parallelPassageGenerator;
        }

        public AreaGenerator Build(string areaType)
        {
            return areaGenerators[areaType];
        }

        public bool HasSpecificGenerator(string areaType)
        {
            return areaGenerators.ContainsKey(areaType);
        }
    }
}
