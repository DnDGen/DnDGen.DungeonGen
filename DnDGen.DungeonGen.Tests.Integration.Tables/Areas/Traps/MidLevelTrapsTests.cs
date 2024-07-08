using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class MidLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.MidLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 2, "Bestow curse trap", 4, 28, 28, "Magic device", "touch trigger, detect chaos", "automatic reset", "spell effect: bestow curse, 5th-level cleric, DC 14 Will save negates")]
        [TestCase(3, 5, "Camouflaged pit trap", 4, 25, 17, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "40' deep, 4d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(6, 7, "Collapsing column", 4, 20, 24, "Mechanical", "touch trigger, attached", "no reset", "BA +15 melee, 6d6 stone block damage")]
        [TestCase(8, 10, "Glyph of warding, blast", 4, 28, 28, "Spell", "spell trigger, see spell description", "no reset", "spell effect: glyph of warding, blast, 5th-level cleric, 2d8 acid damage, DC 14 Reflex save for half damage", "multiple targets: all within 5'")]
        [TestCase(11, 12, "Lightning bolt trap", 4, 28, 28, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: lightning bolt, 5th-level wizard, 5d6 electricity damage, DC 14 Reflex save for half damage")]
        [TestCase(13, 15, "Pit trap", 4, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "80' deep, 8d6 fall damage")]
        [TestCase(16, 18, "Poisoned dart trap", 4, 21, 22, "Mechanical", "location trigger", "manual reset", "BA +15 ranged, 1d4+4 dart damage plus poison", "multiple targets: 1 dart per target within 10'", "poison: small monstrous centipede poison, DC 10 Fortitude save, primary damage is 1d2 Dexterity, secondary damage is 1d2 Dexterity")]
        [TestCase(19, 21, "Sepia snake sigil trap", 4, 28, 28, "Spell", "spell trigger, see spell description", "no reset", "spell effect: sepia snake sigil, 5th-level wizard, DC 14 Reflex save negates")]
        [TestCase(22, 24, "Spiked pit trap", 4, 20, 20, "Mechanical", "location trigger", "automatic reset", "DC 20 Reflex save avoids", "60' deep, 6d6 fall damage", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+5 damage each")]
        [TestCase(25, 26, "Wall scythe trap", 4, 21, 18, "Mechanical", "location trigger", "automatic reset", "BA +20 melee, 2d4+8 scythe damage, crit x4")]
        [TestCase(27, 29, "Water-filled room trap", 4, 17, 23, "Mechanical", "location trigger", "automatic reset", "multiple targets: all targets in a 10' x 10' room", "never miss", "onset delay: 5 rounds", "liquid")]
        [TestCase(30, 32, "Wide-mouth spiked pit trap", 4, 18, 25, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "20' deep, 2d6 fall damage", "multiple targets: first 2 within 5'", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+2 damage each")]
        [TestCase(33, 35, "Camouflaged pit trap", 5, 25, 17, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "50' deep, 5d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(36, 37, "Touchable surface smeared with contact poison", 5, 25, 19, "Mechanical", "touch trigger, attached", "manual reset", "poison: nitharit, DC 13 Fortitude save, no primary damage, secondary damage is 3d6 Constitution")]
        [TestCase(38, 40, "Falling block trap", 5, 20, 25, "Mechanical", "location trigger", "manual reset", "BA +15 melee, 6d6 damage", "multiple targets: all within 5'")]
        [TestCase(41, 43, "Fire trap", 5, 29, 29, "Spell", "spell trigger, see spell description", "no reset", "spell effect: fire trap, 7th-level wizard, 1d4+7 fire damage, DC 16 Reflex save for half damage")]
        [TestCase(44, 46, "Fireball trap", 5, 28, 28, "Magic device", "touch trigger", "automatic reset", "spell effect: fireball, 8th-level wizard, 8d6 fire damage, DC 14 Reflex save for half damage")]
        [TestCase(47, 48, "Flooding room trap", 5, 20, 25, "Mechanical", "proximity trigger", "automatic reset", "no attack roll necessary, room floods in 4 rounds, see Drowning")]
        [TestCase(49, 51, "Fusillade of darts", 5, 19, 25, "Mechanical", "location trigger", "manual reset", "BA +18 ranged, 1d4+1 dart damage", "multiple targets: 1d8 darts per target in a 10' x 10' area")]
        [TestCase(52, 53, "Moving executioner statue", 5, 25, 18, "Mechanical", "location trigger", "automatic reset", "hidden switch bypass: Search DC 25", "BA +16 melee, 1d12+8 greataxe damage, crit x3", "multiple targets: both arms attack")]
        [TestCase(54, 55, "Phantasmal killer trap", 5, 29, 29, "Magic device", "proximity trigger, alarm spell covering entire room", "automatic reset", "spell effect: phantasmal killer, 7th-level wizard, DC 16 Will save for disbelief, DC 16 Fortitude save for partial effect")]
        [TestCase(56, 58, "Pit trap", 5, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "100' deep, 10d6 fall damage")]
        [TestCase(59, 61, "Poison wall spikes", 5, 17, 21, "Mechanical", "location trigger", "manual reset", "BA +16 melee, 1d8+4 spike damage plus poison", "multiple targets: closest 2 within 5'", "poison: Medium monstrous spider venom, DC 12 Fortitude save, primary damage is 1d4 Strength, secondary damage is 1d4 Strength")]
        [TestCase(62, 64, "Spiked pit trap", 5, 21, 20, "Mechanical", "location trigger", "manual reset", "DC 25 Reflex save avoids", "40' deep, 4d6 fall damage", "multiple targets: first 2 within 5'", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+4 damage each")]
        [TestCase(65, 67, "Spiked pit trap", 5, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "80' deep, 8d6 fall damage", "pit spikes: BA +10 melee, 1d4 spikes, 1d4+5 damage each")]
        [TestCase(68, 69, "Ungol dust vapor trap", 5, 20, 16, "Mechanical", "location trigger", "manual reset", "gas", "multiple targets: all within 10' x 10' room", "never miss", "onset delay: 2 rounds", "poison: ungol dust, DC 15 Fortitude save, primary damage is 1 Charisma, secondary damage is 1d6 Charisma and 1 permanent Charisma")]
        [TestCase(70, 71, "Built-to-collapse wall", 6, 14, 16, "Mechanical", "proximity trigger", "no reset", "BA +20 melee, 8d6 stone block damage", "multiple targets: all targets in a 10' x 10' area")]
        [TestCase(72, 74, "Compacting room", 6, 20, 22, "Mechanical", "timed trigger", "automatic reset", "hidden switch bypass: Search DC 25", "walls move together, 12d6 crushing damage", "multiple targets: all targets within a 10' x 10' room", "never miss", "onset delay: 4 rounds")]
        [TestCase(75, 77, "Flame strike trap", 6, 30, 30, "Magic device", "proximity trigger, detect magic", "automatic reset", "spell effect: flame strike, 9th-level cleric, 9d6 fire damage, DC 17 Reflex save for half damage")]
        [TestCase(78, 80, "Fusillade of spears", 6, 26, 20, "Mechanical", "proximity trigger", "repair reset", "BA +21 ranged, 1d8 spear damage", "multiple targets: 1d6 spears per target in a 10' x 10' area")]
        [TestCase(81, 83, "Glyph of warding, blast", 6, 28, 28, "Spell", "spell trigger, see spell description", "no reset", "spell effect: glyph of warding, blast, 16th-level cleric, 8d8 sonic damage, DC 14 Reflex save for half damage", "multiple targets: all targets within 5'")]
        [TestCase(84, 85, "Lightning bolt trap", 6, 28, 28, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: lightning bolt, 10th-level wizard, 10d6 electricity damage, DC 14 Reflex save for half damage")]
        [TestCase(86, 88, "Spiked blocks from ceiling", 6, 24, 20, "Mechanical", "location trigger", "repair reset", "BA +20 melee, 6d6 spike damage", "multiple targets: all targets within a 10' x 10' area")]
        [TestCase(89, 91, "Spiked pit trap", 6, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "100' deep, 10d6 fall damage", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+5 damage each")]
        [TestCase(92, 94, "Whirling poison blades", 6, 20, 20, "Mechanical", "timed trigger", "automatic reset", "hidden lock bypass: Search DC 25, Open Lock DC 30", "BA +10 melee, 1d4+4 dagger damage plus poison, crit 19-20 x2", "poison: purple worm poison, DC 24 Fortitude save, primary damage is 1d6 Strength, secondary damage is 2d6 Strength", "multiple targets: random 3 within 5'")]
        [TestCase(95, 97, "Wide-mouth pit trap", 6, 26, 25, "Mechanical", "location trigger", "manual reset", "DC 25 Reflex save avoids", "40' deep, 4d6 fall damage", "multiple targets: all targets within 10' x 10' area")]
        [TestCase(98, 100, "Wyvern arrow trap", 6, 20, 16, "Mechanical", "proximity trigger", "manual reset", "BA +14 ranged, 1d8 arrow damage plus poison", "poison: wyvern poison, DC 17 Fortitude save, primary damage is 2d6 Constitution, secondary damage is 2d6 Constitution")]
        public void Trap(int lower, int upper, string name, int challengeRating, int searchDC, int disableDeviceDC, params string[] descriptions)
        {
            var description = string.Join("/", descriptions);
            base.AreaPercentile(lower, upper, name, description, challengeRating.ToString(), searchDC, disableDeviceDC);
        }
    }
}
