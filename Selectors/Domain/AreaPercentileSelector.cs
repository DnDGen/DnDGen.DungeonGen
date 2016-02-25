using DungeonGen.Common;
using System;
using System.Linq;

namespace DungeonGen.Selectors.Domain
{
    public class AreaPercentileSelector : IAreaPercentileSelector
    {
        private IPercentileSelector innerSelector;

        public AreaPercentileSelector(IPercentileSelector innerSelector)
        {
            this.innerSelector = innerSelector;
        }

        public Area SelectFrom(string tableName)
        {
            var result = innerSelector.SelectFrom(tableName);
            var area = new Area();

            area.Type = GetAreaType(result);
            area.Description = GetDescription(result);
            area.Length = GetLength(result);
            area.Width = GetWidth(result);

            var contents = GetContents(result);
            if (string.IsNullOrEmpty(contents) == false)
                area.Contents.Miscellaneous = area.Contents.Miscellaneous.Union(new[] { contents });

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

        private string GetDescription(string result)
        {
            var descriptionStartIndex = result.IndexOf('(');
            var descriptionEndIndex = result.IndexOf(')');

            if (descriptionStartIndex < 0)
                return string.Empty;

            return result.Substring(descriptionStartIndex + 1, descriptionEndIndex - descriptionStartIndex - 1);
        }

        private int GetLength(string result)
        {
            var lengthStartIndex = result.IndexOf('{');
            if (lengthStartIndex < 0)
                return 0;

            var lengthEndIndex = result.IndexOf('x', lengthStartIndex);
            var length = result.Substring(lengthStartIndex + 1, lengthEndIndex - lengthStartIndex - 1);

            return Convert.ToInt32(length);
        }

        private int GetWidth(string result)
        {
            var lengthStartIndex = result.IndexOf('{');
            if (lengthStartIndex < 0)
                return 0;

            var widthStartIndex = result.IndexOf('x', lengthStartIndex);
            var widthEndIndex = result.IndexOf('}');
            var width = result.Substring(widthStartIndex + 1, widthEndIndex - widthStartIndex - 1);

            return Convert.ToInt32(width);
        }

        private string GetContents(string result)
        {
            var contentsStartIndex = result.IndexOf('[');
            var contentsEndIndex = result.IndexOf(']');

            if (contentsStartIndex < 0)
                return string.Empty;

            return result.Substring(contentsStartIndex + 1, contentsEndIndex - contentsStartIndex - 1);
        }
    }
}
