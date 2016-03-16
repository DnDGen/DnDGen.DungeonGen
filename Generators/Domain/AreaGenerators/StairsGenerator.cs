using DungeonGen.Common;
using DungeonGen.Selectors;
using DungeonGen.Tables;
using RollGen;
using System.Collections.Generic;

namespace DungeonGen.Generators.Domain.AreaGenerators
{
    public class StairsGenerator : AreaGenerator
    {
        private IAreaPercentileSelector areaPercentileSelector;
        private Dice dice;
        private AreaGenerator chamberGenerator;

        public StairsGenerator(IAreaPercentileSelector areaPercentileSelector, Dice dice, AreaGenerator chamberGenerator)
        {
            this.areaPercentileSelector = areaPercentileSelector;
            this.dice = dice;
            this.chamberGenerator = chamberGenerator;
        }

        public IEnumerable<Area> Generate(int dungeonLevel, int partyLevel)
        {
            var stairs = areaPercentileSelector.SelectFrom(TableNameConstants.Stairs);

            var endLevel = dungeonLevel + stairs.Length;
            if (endLevel < 1)
                return new[] { GetDeadEnd() };

            var directionDescription = GetDirectionDescription(dungeonLevel, endLevel);

            var allStairAreas = new List<Area>();
            allStairAreas.Add(stairs);

            return allStairAreas;
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
                return $"Down to level {endLevel}";

            return $"Up to level {endLevel}";
        }
    }
}
