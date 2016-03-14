using DungeonGen.Common;
using RollGen;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen.Selectors.Domain
{
    public class AreaPercentileSelector : IAreaPercentileSelector
    {
        private IPercentileSelector innerSelector;
        private Dice dice;

        public AreaPercentileSelector(IPercentileSelector innerSelector, Dice dice)
        {
            this.innerSelector = innerSelector;
            this.dice = dice;
        }

        public Area SelectFrom(string tableName)
        {
            var result = innerSelector.SelectFrom(tableName);
            var area = new Area();

            area.Type = GetAreaType(result);
            area.Descriptions = GetDescriptions(result);
            area.Length = GetLength(result);
            area.Width = GetWidth(result);
            area.Contents.Miscellaneous = GetContents(result);

            return area;
        }

        private string GetAreaType(string result)
        {
            var descriptionIndex = result.IndexOf('(');
            var contentsIndex = result.IndexOf('[');
            var dimensionsIndex = result.IndexOf('{');
            var indices = new[] { descriptionIndex, contentsIndex, dimensionsIndex };
            var validIndices = indices.Where(i => i >= 0);

            if (validIndices.Any() == false)
                return result;

            var firstIndex = validIndices.Min();
            var areaType = result.Substring(0, firstIndex);

            return areaType;
        }

        private IEnumerable<string> GetDescriptions(string result)
        {
            var descriptionStartIndex = result.IndexOf('(');
            var descriptionEndIndex = result.IndexOf(')');

            if (descriptionStartIndex < 0)
                return Enumerable.Empty<string>();

            var descriptions = result.Substring(descriptionStartIndex + 1, descriptionEndIndex - descriptionStartIndex - 1);

            return descriptions.Split('/');
        }

        private int GetLength(string result)
        {
            var lengthStartIndex = result.IndexOf('{');
            if (lengthStartIndex < 0)
                return 0;

            var lengthEndIndex = result.IndexOf('x', lengthStartIndex);
            var length = result.Substring(lengthStartIndex + 1, lengthEndIndex - lengthStartIndex - 1);

            return dice.Roll(length);
        }

        private int GetWidth(string result)
        {
            var lengthStartIndex = result.IndexOf('{');
            if (lengthStartIndex < 0)
                return 0;

            var widthStartIndex = result.IndexOf('x', lengthStartIndex);
            var widthEndIndex = result.IndexOf('}');
            var width = result.Substring(widthStartIndex + 1, widthEndIndex - widthStartIndex - 1);

            return dice.Roll(width);
        }

        private IEnumerable<string> GetContents(string result)
        {
            var contentsStartIndex = result.IndexOf('[');
            var contentsEndIndex = result.IndexOf(']');

            if (contentsStartIndex < 0)
                return Enumerable.Empty<string>();

            var contents = result.Substring(contentsStartIndex + 1, contentsEndIndex - contentsStartIndex - 1);

            return contents.Split('/');
        }
    }
}
