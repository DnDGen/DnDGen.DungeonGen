using NUnit.Framework;

namespace DungeonGen.Tests.Integration.Tables
{
    [TestFixture]
    public abstract class AreaPercentileTests : PercentileTests
    {
        public virtual void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, int width)
        {
            AreaPercentile(lower, upper, areaType, description, contents, length.ToString(), width.ToString());
        }

        public virtual void AreaPercentile(int lower, int upper, string areaType, string description, string contents, int length, string width)
        {
            AreaPercentile(lower, upper, areaType, description, contents, length.ToString(), width);
        }

        public virtual void AreaPercentile(int lower, int upper, string areaType, string description, string contents, string length, int width)
        {
            AreaPercentile(lower, upper, areaType, description, contents, length, width.ToString());
        }

        public virtual void AreaPercentile(int lower, int upper, string areaType, string description, string contents, string length, string width)
        {
            var percentileContents = areaType;

            if (string.IsNullOrEmpty(description) == false)
                percentileContents += string.Format("({0})", description);

            if (string.IsNullOrEmpty(contents) == false)
                percentileContents += string.Format("[{0}]", contents);

            if (length != "0" || width != "0")
                percentileContents += string.Format("{{{0}x{1}}}", length, width);

            Percentile(percentileContents, lower, upper);
        }
    }
}
