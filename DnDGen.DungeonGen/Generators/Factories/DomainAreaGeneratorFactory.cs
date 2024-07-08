using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Models;
using DnDGen.Infrastructure.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.Factories
{
    internal class DomainAreaGeneratorFactory : AreaGeneratorFactory
    {
        private readonly JustInTimeFactory innerFactory;
        private readonly IEnumerable<string> specificGenerators;

        public DomainAreaGeneratorFactory(JustInTimeFactory innerFactory)
        {
            this.innerFactory = innerFactory;

            specificGenerators =
            [
                AreaTypeConstants.Chamber,
                AreaTypeConstants.Door,
                AreaTypeConstants.Room,
                AreaTypeConstants.SidePassage,
                AreaTypeConstants.Stairs,
                AreaTypeConstants.Turn,
                SidePassageConstants.ParallelPassage,
            ];
        }

        public AreaGenerator Build(string areaType)
        {
            var generator = innerFactory.Build<AreaGenerator>(areaType);
            return generator;
        }

        public bool HasSpecificGenerator(string areaType)
        {
            return specificGenerators.Contains(areaType);
        }
    }
}
