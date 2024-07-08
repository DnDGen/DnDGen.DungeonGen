using DnDGen.DungeonGen.Models;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Unit.Models
{
    [TestFixture]
    public class ContentsConstantsTests
    {
        [TestCase(ContentsConstants.Chasm, "Chasm bisects the passage")]
        [TestCase(ContentsConstants.Gallery, "columns 10' on right and left, supporting 10' wide upper galleries 20' up")]
        [TestCase(ContentsConstants.GalleryStairs_Beginning, "Stairs up to the gallery at the beginning of the hall")]
        [TestCase(ContentsConstants.GalleryStairs_End, "Stairs up to the gallery at the end of the hall")]
        [TestCase(ContentsConstants.LikesAlignment, "Talking pool (grants wish to ALIGNMENT characters, deals 1d20 points of damage to anyone else who speaks to it)")]
        [TestCase(ContentsConstants.MagicPool, "Magic pool")]
        [TestCase(ContentsConstants.River, "River bisects the passage")]
        [TestCase(ContentsConstants.Stream, "Stream bisects the passage")]
        [TestCase(ContentsConstants.TeleportationPool, "Wading into the pool teleports the character DESTINATION")]
        public void Constant(string constant, string value)
        {
            Assert.That(constant, Is.EqualTo(value));
        }
    }
}
