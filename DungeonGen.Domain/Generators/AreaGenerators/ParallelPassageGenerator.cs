using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class ParallelPassageGenerator : AreaGenerator
    {
        private AreaGenerator hallGenerator;

        public ParallelPassageGenerator(AreaGenerator hallGenerator)
        {
            this.hallGenerator = hallGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var leftPassage = hallGenerator.Generate(dungeonLevel, partyLevel).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, partyLevel).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            var maxWidth = Math.Max(leftPassage.Width, rightPassage.Width);

            leftPassage.Width = maxWidth;
            rightPassage.Width = maxWidth;

            return new[] { leftPassage, rightPassage };
        }
    }
}
