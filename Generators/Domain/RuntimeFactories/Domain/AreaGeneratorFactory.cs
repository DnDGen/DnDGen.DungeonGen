using DungeonGen.Common;
using System.Collections.Generic;

namespace DungeonGen.Generators.Domain.RuntimeFactories.Domain
{
    public class AreaGeneratorFactory : IAreaGeneratorFactory
    {
        private readonly Dictionary<string, AreaGenerator> areaGenerators;

        public AreaGeneratorFactory(AreaGenerator chamberGenerator, AreaGenerator doorGenerator, AreaGenerator hallGenerator, AreaGenerator roomGenerator, AreaGenerator sidePassageGenerator, AreaGenerator stairsGenerator, AreaGenerator turnGenerator)
        {
            areaGenerators = new Dictionary<string, AreaGenerator>();
            areaGenerators[AreaTypeConstants.Chamber] = chamberGenerator;
            areaGenerators[AreaTypeConstants.Door] = doorGenerator;
            areaGenerators[AreaTypeConstants.Hall] = hallGenerator;
            areaGenerators[AreaTypeConstants.Room] = roomGenerator;
            areaGenerators[AreaTypeConstants.SidePassage] = sidePassageGenerator;
            areaGenerators[AreaTypeConstants.Stairs] = stairsGenerator;
            areaGenerators[AreaTypeConstants.Turn] = turnGenerator;
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
