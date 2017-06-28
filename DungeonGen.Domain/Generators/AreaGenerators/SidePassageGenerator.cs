using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using EncounterGen.Generators;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class SidePassageGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.SidePassage; }
        }

        private IPercentileSelector percentileSelector;
        private AreaGeneratorFactory areaGeneratorFactory;

        public SidePassageGenerator(IPercentileSelector percentileSelector, AreaGeneratorFactory areaGeneratorFactory)
        {
            this.percentileSelector = percentileSelector;
            this.areaGeneratorFactory = areaGeneratorFactory;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, EncounterSpecifications environment)
        {
            var sidePassageType = percentileSelector.SelectFrom(TableNameConstants.SidePassages);

            switch (sidePassageType)
            {
                case SidePassageConstants.YIntersection: return GenerateYIntersection(dungeonLevel, environment);
                case SidePassageConstants.XIntersection: return GenerateXIntersection(dungeonLevel, environment);
                case SidePassageConstants.TIntersection: return GenerateTIntersection(dungeonLevel, environment);
                case SidePassageConstants.CrossIntersection: return GenerateCrossIntersection(dungeonLevel, environment);
                default: return GenerateSidePassage(dungeonLevel, environment, sidePassageType);
            }
        }

        private IEnumerable<Area> GenerateYIntersection(int dungeonLevel, EncounterSpecifications environment)
        {
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var leftPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesAhead });

            var rightPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesAhead });

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateXIntersection(int dungeonLevel, EncounterSpecifications environment)
        {
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var leftBehindPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftBehindPassage.Descriptions = leftBehindPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesBehind });

            var leftAheadPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftAheadPassage.Descriptions = leftAheadPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesAhead });

            var rightAheadPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightAheadPassage.Descriptions = rightAheadPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesAhead });

            var rightBehindPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightBehindPassage.Descriptions = rightBehindPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesBehind });

            return new[] { leftBehindPassage, leftAheadPassage, rightAheadPassage, rightBehindPassage };
        }

        private IEnumerable<Area> GenerateTIntersection(int dungeonLevel, EncounterSpecifications environment)
        {
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var leftPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateCrossIntersection(int dungeonLevel, EncounterSpecifications environment)
        {
            var originalHall = GetOriginalHall();
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var leftPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            return new[] { leftPassage, originalHall, rightPassage };
        }

        private IEnumerable<Area> GenerateSidePassage(int dungeonLevel, EncounterSpecifications environment, string sidePassageType)
        {
            var originalHall = GetOriginalHall();
            var hallGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Hall);

            var sidePassage = hallGenerator.Generate(dungeonLevel, environment).Single();
            sidePassage.Descriptions = sidePassage.Descriptions.Union(new[] { sidePassageType });

            return new[] { originalHall, sidePassage };
        }

        private Area GetOriginalHall()
        {
            return new Area
            {
                Type = AreaTypeConstants.Hall,
                Length = 30
            };
        }
    }
}
