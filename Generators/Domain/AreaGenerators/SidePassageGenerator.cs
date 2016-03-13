using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class SidePassageGenerator : AreaGenerator
    {
        private IPercentileSelector percentileSelector;
        private AreaGenerator hallGenerator;

        public SidePassageGenerator(IPercentileSelector percentileSelector, AreaGenerator hallGenerator)
        {
            this.percentileSelector = percentileSelector;
            this.hallGenerator = hallGenerator;
        }

        public IEnumerable<Area> Generate(int level)
        {
            var sidePassageType = percentileSelector.SelectFrom(TableNameConstants.SidePassages);

            switch (sidePassageType)
            {
                case SidePassageConstants.YIntersection: return GenerateYIntersection(level);
                case SidePassageConstants.XIntersection: return GenerateXIntersection(level);
                case SidePassageConstants.TIntersection: return GenerateTIntersection(level);
                case SidePassageConstants.CrossIntersection: return GenerateCrossIntersection(level);
                default: return GenerateSidePassage(level, sidePassageType);
            }
        }

        private IEnumerable<Area> GenerateYIntersection(int level)
        {
            var leftPassage = hallGenerator.Generate(level).Single();
            leftPassage.Description = SidePassageConstants.Left45DegreesAhead;

            var rightPassage = hallGenerator.Generate(level).Single();
            rightPassage.Description = SidePassageConstants.Right45DegreesAhead;

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateXIntersection(int level)
        {
            var leftBehindPassage = hallGenerator.Generate(level).Single();
            leftBehindPassage.Description = SidePassageConstants.Left45DegreesBehind;

            var leftAheadPassage = hallGenerator.Generate(level).Single();
            leftAheadPassage.Description = SidePassageConstants.Left45DegreesAhead;

            var rightAheadPassage = hallGenerator.Generate(level).Single();
            rightAheadPassage.Description = SidePassageConstants.Right45DegreesAhead;

            var rightBehindPassage = hallGenerator.Generate(level).Single();
            rightBehindPassage.Description = SidePassageConstants.Right45DegreesBehind;

            return new[] { leftBehindPassage, leftAheadPassage, rightAheadPassage, rightBehindPassage };
        }

        private IEnumerable<Area> GenerateTIntersection(int level)
        {
            var leftPassage = hallGenerator.Generate(level).Single();
            leftPassage.Description = SidePassageConstants.Left90Degrees;

            var rightPassage = hallGenerator.Generate(level).Single();
            rightPassage.Description = SidePassageConstants.Right90Degrees;

            return new[] { leftPassage, rightPassage };
        }

        private IEnumerable<Area> GenerateCrossIntersection(int level)
        {
            var originalHall = GetOriginalHall();

            var leftPassage = hallGenerator.Generate(level).Single();
            leftPassage.Description = SidePassageConstants.Left90Degrees;

            var rightPassage = hallGenerator.Generate(level).Single();
            rightPassage.Description = SidePassageConstants.Right90Degrees;

            return new[] { leftPassage, originalHall, rightPassage };
        }

        private IEnumerable<Area> GenerateSidePassage(int level, string sidePassageType)
        {
            var originalHall = GetOriginalHall();

            var sidePassage = hallGenerator.Generate(level).Single();
            sidePassage.Description = sidePassageType;

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
