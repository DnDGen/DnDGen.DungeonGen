using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class SidePassageGenerator : AreaGenerator
    {
        private IPercentileSelector percentileSelector;
        private AreaGenerator hallGenerator;

        public SidePassageGenerator(IPercentileSelector percentileSelector, AreaGenerator hallGenerator)
        {
            this.percentileSelector = percentileSelector;
            this.hallGenerator = hallGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
        {
            var sidePassageType = percentileSelector.SelectFrom(TableNameConstants.SidePassages);

            switch (sidePassageType)
            {
                case SidePassageConstants.YIntersection: return GenerateYIntersection(dungeonLevel, partyLevel, temperature);
                case SidePassageConstants.XIntersection: return GenerateXIntersection(dungeonLevel, partyLevel, temperature);
                case SidePassageConstants.TIntersection: return GenerateTIntersection(dungeonLevel, partyLevel, temperature);
                case SidePassageConstants.CrossIntersection: return GenerateCrossIntersection(dungeonLevel, partyLevel, temperature);
                default: return GenerateSidePassage(dungeonLevel, partyLevel, sidePassageType, temperature);
            }
        }

        private IEnumerable<Area> GenerateYIntersection(int dungeonLevel, int partyLevel, string temperature)
        {
            var leftPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesAhead });

            var rightPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesAhead });

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateXIntersection(int dungeonLevel, int partyLevel, string temperature)
        {
            var leftBehindPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            leftBehindPassage.Descriptions = leftBehindPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesBehind });

            var leftAheadPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            leftAheadPassage.Descriptions = leftAheadPassage.Descriptions.Union(new[] { SidePassageConstants.Left45DegreesAhead });

            var rightAheadPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            rightAheadPassage.Descriptions = rightAheadPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesAhead });

            var rightBehindPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            rightBehindPassage.Descriptions = rightBehindPassage.Descriptions.Union(new[] { SidePassageConstants.Right45DegreesBehind });

            return new[] { leftBehindPassage, leftAheadPassage, rightAheadPassage, rightBehindPassage };
        }

        private IEnumerable<Area> GenerateTIntersection(int dungeonLevel, int partyLevel, string temperature)
        {
            var leftPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateCrossIntersection(int dungeonLevel, int partyLevel, string temperature)
        {
            var originalHall = GetOriginalHall();

            var leftPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            leftPassage.Descriptions = leftPassage.Descriptions.Union(new[] { SidePassageConstants.Left90Degrees });

            var rightPassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
            rightPassage.Descriptions = rightPassage.Descriptions.Union(new[] { SidePassageConstants.Right90Degrees });

            return new[] { leftPassage, originalHall, rightPassage };
        }

        private IEnumerable<Area> GenerateSidePassage(int dungeonLevel, int partyLevel, string sidePassageType, string temperature)
        {
            var originalHall = GetOriginalHall();

            var sidePassage = hallGenerator.Generate(dungeonLevel, partyLevel, temperature).Single();
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
