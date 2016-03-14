using DungeonGen.Common;
using NUnit.Framework;

namespace DungeonGen.Tests.Unit.Common
{
    [TestFixture]
    public class ContentsConstantsTests
    {
        [TestCase(ContentsConstants.Gallery, "columns 10' on right and left, supporting 10' wide upper galleries 20' up")]
        [TestCase(ContentsConstants.River, "River bisects the passage")]
        [TestCase(ContentsConstants.Stream, "Stream bisects the passage")]
        [TestCase(ContentsConstants.Chasm, "Chasm bisects the passage")]
        [TestCase(ContentsConstants.GalleryStairs_Beginning, "Stairs up to the gallery at the beginning of the hall")]
        [TestCase(ContentsConstants.GalleryStairs_End, "Stairs up to the gallery at the end of the hall")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}
