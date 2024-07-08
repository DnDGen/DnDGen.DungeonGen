using DnDGen.DungeonGen.Generators.Factories;
using DnDGen.DungeonGen.Models;
using DnDGen.EncounterGen.Generators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGen.DungeonGen.Generators.AreaGenerators
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
            leftPassage.Descriptions = leftPassage.Descriptions.Union([SidePassageConstants.Left90Degrees]);

            var rightPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union([SidePassageConstants.Right90Degrees]);

            var maxWidth = Math.Max(leftPassage.Width, rightPassage.Width);

            leftPassage.Width = maxWidth;
            rightPassage.Width = maxWidth;

            return [leftPassage, rightPassage];
        }
    }
}
