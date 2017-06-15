using DungeonGen.Domain.Generators.AreaGenerators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.Factories
{
    internal class DomainAreaGeneratorFactory : AreaGeneratorFactory
    {
        private readonly JustInTimeFactory innerFactory;
        private readonly IEnumerable<string> specificGenerators;

        public DomainAreaGeneratorFactory(JustInTimeFactory innerFactory)
        {
            this.innerFactory = innerFactory;

            specificGenerators = new[]
            {
                AreaTypeConstants.Chamber,
                AreaTypeConstants.Door,
                AreaTypeConstants.Room,
                AreaTypeConstants.SidePassage,
                AreaTypeConstants.Stairs,
                AreaTypeConstants.Turn,
                SidePassageConstants.ParallelPassage,
            };
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
