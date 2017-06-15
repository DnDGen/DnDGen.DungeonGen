using DungeonGen.Domain.Generators.Factories;
using DungeonGen.Domain.Selectors;
using DungeonGen.Domain.Tables;
using RollGen;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Domain.Generators.AreaGenerators
{
    internal class StairsGenerator : AreaGenerator
    {
        public string AreaType
        {
            get { return AreaTypeConstants.Stairs; }
        }

        private readonly IAreaPercentileSelector areaPercentileSelector;
        private readonly Dice dice;
        private readonly AreaGeneratorFactory areaGeneratorFactory;

        public StairsGenerator(IAreaPercentileSelector areaPercentileSelector, Dice dice, AreaGeneratorFactory areaGeneratorFactory)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.dice = dice;
            this.areaGeneratorFactory = areaGeneratorFactory;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel, string temperature)
        {
            var stairs = areaPercentileSelector.SelectFrom(TableNameConstants.Stairs);
            var endLevel = dungeonLevel + stairs.Length;
            var passiveStairs = new[] { DescriptionConstants.Chimney, DescriptionConstants.TrapDoor };
            var allStairAreas = new List<Area>();

            if (stairs.Descriptions.Intersect(passiveStairs).Any())
            {
                var originalHall = GetOriginalHall();
                allStairAreas.Add(originalHall);

                if (endLevel < 1)
                    return allStairAreas;
            }
            else if (endLevel < 1)
            {
                return new[] { GetDeadEnd() };
            }

            var directionDescription = GetDirectionDescription(dungeonLevel, endLevel);
            stairs.Descriptions = stairs.Descriptions.Union(new[] { directionDescription });

            allStairAreas.Add(stairs);

            if (stairs.Contents.Miscellaneous.Contains(AreaTypeConstants.Chamber))
            {
                stairs.Contents.Miscellaneous = stairs.Contents.Miscellaneous.Except(new[] { AreaTypeConstants.Chamber });

                var chamberGenerator = areaGeneratorFactory.Build(AreaTypeConstants.Chamber);
                var chambers = chamberGenerator.Generate(dungeonLevel, partyLevel, temperature);

                allStairAreas.AddRange(chambers);
            }
            else if (dice.Roll().Percentile().AsSum() <= stairs.Width)
            {
                var deadEnd = GetDeadEnd();
                allStairAreas.Add(deadEnd);
            }

            stairs.Length = 0;
            stairs.Width = 0;

            return allStairAreas;
        }

        private Area GetOriginalHall()
        {
            return new Area
            {
                Type = AreaTypeConstants.Hall,
                Length = 30
            };
        }

        private Area GetDeadEnd()
        {
            var deadEnd = new Area();
            deadEnd.Type = AreaTypeConstants.DeadEnd;

            return deadEnd;
        }

        private string GetDirectionDescription(int dungeonLevel, int endLevel)
        {
            if (dungeonLevel > endLevel)
                return $"Up to level {endLevel}";

            return $"Down to level {endLevel}";
        }
    }
}
