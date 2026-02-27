using DnDGen.DungeonGen.Generators.AreaGenerators;
using DnDGen.DungeonGen.Models;
using DnDGen.Infrastructure.Factories;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.Factories
{
    internal class DomainAreaGeneratorFactory(JustInTimeFactory innerFactory) : AreaGeneratorFactory
    {
        private readonly IEnumerable<string> specificGenerators =
        [
            AreaTypeConstants.Chamber,
            AreaTypeConstants.Door,
            AreaTypeConstants.Room,
            AreaTypeConstants.SidePassage,
            AreaTypeConstants.Stairs,
            AreaTypeConstants.Turn,
            SidePassageConstants.ParallelPassage,
        ];

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
