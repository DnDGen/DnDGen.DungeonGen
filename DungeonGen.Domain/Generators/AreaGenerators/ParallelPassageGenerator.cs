using DungeonGen.Domain.Generators.Factories;
using EncounterGen.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class ParallelPassageGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Hall; }
        }

        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public ParallelPassageGenerator(AreaGeneratorFactory areaGeneratorFactory)
        {
            this.areaGeneratorFactory = areaGeneratorFactory;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var leftPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            var maxWidth = Math.Max(leftPassage.Width, rightPassage.Width);

            leftPassage.Width = maxWidth;
            rightPassage.Width = maxWidth;

            return new[] { leftPassage, rightPassage };
        }
    }
}
