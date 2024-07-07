using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class HighLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.HighLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 4, "Acid fog trap", 7, 31, 31, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: acid fog, 11th-level wizard, 2d6 acid damage per round for 11 rounds")]
        [TestCase(5, 7, "Blade barrier trap", 7, 31, 31, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: blade barrier, 11th-level cleric, 11d6 slashing damage, DC 19 Reflex save for half damage")]
        [TestCase(8, 10, "Burnt othur vapor trap", 7, 21, 21, "Mechanical", "location trigger", "repair reset", "gas", "multiple targets: all targets within a 10' x 10' room", "never miss", "onset delay: 3 rounds", "poison: burnt othur fumes, DC 18 Fortitude save, primary damage is 1 permanent Constitution, secondary damage is 3d6 Constitution")]
        [TestCase(11, 14, "Chain lightning trap", 7, 31, 31, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: chain lightning, 11th-level wizard, 11d6 electricity damage to closest target, 5d6 electricity damage to each of up to 11 secondary targets, DC 19 Reflex save for half damage")]
        [TestCase(15, 17, "Evard's black tentacles trap", 7, 29, 29, "Magic device", "proximity trigger, alarm spell", "no reset", "spell effect: evard's black tentacles, 7th-level wizard, 1d4+7 tentacles, BA +7 melee per tentacle, 1d6+4 damage per tentacle", "multiple targets: up to 6 tentacles per target within 5'")]
        [TestCase(18, 20, "Fusillade of greenblood oil darts", 7, 25, 25, "Mechanical", "location trigger", "manual reset", "BA +18 ranged, 1d4+1 dart damage plus poison", "poison: greenblood oil, DC 13 Fortitude save, primary damage is 1 Constitution, secondary damage is 1d2 Constitution", "multiple targets: 1d8 darts per target in a 10' x 10' area")]
        [TestCase(21, 23, "Touchable surface covered in dragon bile", 7, 27, 16, "Mechanical", "touch trigger, attached", "no reset", "poison: dragon bile, DC 26 Fortitude save, primary damage is 3d6 Strength, no secondary damage")]
        [TestCase(24, 26, "Summon monster VI trap", 7, 31, 31, "Magic device", "proximity trigger, alarm spell", "no reset", "spell effect: summon monster VI, 11th-level wizard")]
        [TestCase(27, 30, "Water-filled room", 7, 20, 25, "Mechanical", "location trigger", "manual reset", "multiple targets: all within a 10' x 10' room", "never miss", "onset delay: 3 rounds", "water")]
        [TestCase(31, 33, "Well-camouflaged pit trap", 7, 27, 18, "Mechanical", "location trigger", "repair reset", "DC 25 Reflex save avoids", "70' deep, 7d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(34, 36, "Deathblade wall scythe", 8, 24, 19, "Mechanical", "touch trigger", "manual reset", "BA +16 melee, 2d4+8 scythe damage plus poison", "poison: deathblade, DC 20 Fortitude save, primary damage is 1d6 Constitution, secondary damage is 2d6 Constitution")]
        [TestCase(37, 39, "Destruction trap", 8, 32, 32, "Magic device", "touch trigger, alarm spell", "automatic reset", "spell effect: destruction, 13th-level cleric, DC 20 Fortitude save for 10d6 damage")]
        [TestCase(40, 42, "Earthquake trap", 8, 32, 32, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: earthquake, 13th-level cleric, 65' radius, DC 15 or 20 Reflex save, depending on terrain")]
        [TestCase(43, 46, "Insanity mist vapor trap", 8, 25, 20, "Mechanical", "location trigger", "repair reset", "gas", "never miss", "onset delay: 1 round", "poison: insanity mist, DC 15 Fortitude save, primary damage is 1d4 Wisdom, secondary damage is 2d6 Wisdom", "multiple targets: all targets within a 10' x 10' room")]
        [TestCase(47, 49, "Melf's acid arrow trap", 8, 27, 27, "Magic device", "visual trigger, true seeing", "automatic reset", "multiple traps: two simultaneous Melf's acid arrow traps", "BA +9 ranged touch", "spell effect: Melf's acid arrow, 18th-level wizard, 2d4 acid damage for 7 rounds")]
        [TestCase(50, 52, "Power word stun trap", 8, 32, 32, "Magic device", "touch trigger", "no reset", "spell effect: power word stun, 13th-level wizard")]
        [TestCase(53, 55, "Prismatic spray trap", 8, 32, 32, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: prismatic spray, 13th-level wizard, DC 20 Reflex, Fortitude, or Will save, depending on effect")]
        [TestCase(56, 59, "Reverse gravity trap", 8, 32, 32, "Magic device", "proximity trigger, alarm, 10' area", "automatic reset", "spell effect: reverse gravity, 13th-level wizard, 1d6 fall damage per 10' from hitting ceiling, 1d6 fall damage per 10' from hitting floor after spell ends, DC 20 Reflex save avoids damage")]
        [TestCase(60, 62, "Well-camouflaged pit trap", 8, 27, 18, "Mechanical", "location trigger", "repair reset", "DC 20 Reflex save avoids", "100' deep, 10d6 fall damage")]
        [TestCase(63, 65, "Word of chaos trap", 8, 32, 32, "Magic device", "proximity trigger, detect law", "automatic reset", "spell effect: word of chaos, 13th-level cleric")]
        [TestCase(66, 68, "Touchable surface smeared with contact poison", 9, 18, 26, "Mechanical", "touch trigger, attached", "manual reset", "poison: black lotus extract, DC 20 Fortitude save, primary damage is 3d6 Constitution, secondary damage is 3d6 Constitution")]
        [TestCase(69, 71, "Dropping ceiling", 9, 20, 16, "Mechanical", "location trigger", "repair reset", "ceiling moves down, 12d6 crushing damage", "multiple targets: all targets in a 10' x 10' room", "never miss", "onset delay: 1 round")]
        [TestCase(72, 74, "Incendiary cloud trap", 9, 33, 33, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: incendiary cloud, 15th-level wizard, 4d6 damage per round for 15 rounds, DC 22 Reflex save for half damage")]
        [TestCase(75, 77, "Wide-mouth pit trap", 9, 25, 25, "Mechanical", "location trigger", "manual reset", "DC 25 Reflex save avoids", "100' deep, 10d6 fall damage", "multiple targets: all targets within a 10' x 10' area")]
        [TestCase(78, 80, "Wide mouth spiked pit with poisoned spikes", 9, 20, 20, "Mechanical", "location trigger", "manual reset", "hidden lock bypass: Search DC 25, Open Lock DC 30", "DC 20 Reflex save avoids", "70' deep, 7d6 fall damage", "multiple targets: all targets within a 10' x 10' area", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+5 damage plus poison each", "poison: giant wasp poison, DC 14 Fortitude save, primary damage is 1d6 Dexterity, secondary damage is 1d6 Dexterity")]
        [TestCase(81, 84, "Crushing room", 10, 22, 20, "Mechanical", "location trigger", "automatic reset", "walls move together, 16d6 crushing damage", "multiple targets: all targets in a 10' x 10' room", "never miss", "onset delay: 2 rounds")]
        [TestCase(85, 88, "Crushing wall trap", 10, 20, 25, "Mechanical", "location trigger", "automatic reset", "no attack roll required, 18d6 crushing damage")]
        [TestCase(89, 91, "Energy drain trap", 10, 34, 34, "Magic device", "visual trigger, true seeing", "automatic reset", "BA +8 ranged touch", "spell effect: energy drain, 17th-level wizard, 2d4 negative levels for 24 hours, DC 23 Fortitude save negates")]
        [TestCase(92, 94, "Forcecage and summon monster VII trap", 10, 32, 32, "Magic device", "proximity trigger, alarm spell", "automatic reset", "multiple traps: one forcecage trap and one summon monster VII trap that summons a Hamatula", "spell effect: forcecage, 13th-level wizard", "spell effect: summon monster VII, 13th-level wizard, summons Hamatula")]
        [TestCase(95, 97, "Poisoned spiked pit trap", 10, 16, 25, "Mechanical", "location trigger", "manual reset", "hidden lock bypass: Search DC 25, Open Lock DC 30", "DC 20 Reflex save avoids", "50' deep, 5d6 fall damage", "multiple targets: first 2 within 5'", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+5 damage plus poison each", "poison: purple worm poison, DC 24 Fortitude save, primary damage is 1d6 Strength, secondary damage is 2d6 Strength")]
        [TestCase(98, 100, "Wail of the banshee trap", 10, 34, 34, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: wail of the banshee, 17th-level wizard, DC 23 Fortitude save negates", "multiple targets: up to 17 creatures")]
        public void Trap(int lower, int upper, string name, int challengeRating, int searchDC, int disableDeviceDC, params string[] descriptions)
        {
            var description = string.Join("/", descriptions);
            base.AreaPercentile(lower, upper, name, description, challengeRating.ToString(), searchDC, disableDeviceDC);
        }
    }
}
