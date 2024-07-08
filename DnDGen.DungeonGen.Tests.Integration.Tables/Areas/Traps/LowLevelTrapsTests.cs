using DnDGen.DungeonGen.Tables;
using NUnit.Framework;

namespace DnDGen.DungeonGen.Tests.Integration.Tables.Areas.Traps
{
    [TestFixture]
    public class LowLevelTrapsTests : AreaPercentileTests
    {
        protected override string tableName
        {
            get
            {
                return TableNameConstants.LowLevelTraps;
            }
        }

        [Test]
        public override void TableIsComplete()
        {
            AssertTableIsComplete();
        }

        [TestCase(1, 3, "Basic arrow trap", 1, 20, 20, "Mechanical", "proximity trigger", "manual reset", "BA +10 ranged, 1d6 arrow damage, crit x3")]
        [TestCase(4, 6, "Camouflaged pit trap", 1, 20, 23, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "10' deep, 1d6 fall damage")]
        [TestCase(7, 9, "Deeper pit trap", 1, 20, 23, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "20' deep, 2d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(10, 11, "Touchable surface smeared with contact poison", 1, 19, 19, "Mechanical", "touch trigger, attached", "manual reset", "poison: carrion crawler brain juice, DC 13 Fortitude save, primary is paralysis for 2d6 rounds, no secondary damage")]
        [TestCase(12, 14, "Fusillade of darts", 1, 14, 20, "Mechanical", "location trigger", "manual reset", "BA +10 ranged, 1d4+1 dart damage", "multiple targets: 1d4 darts at each target within 5'")]
        [TestCase(15, 16, "Poisoned dart trap", 1, 20, 18, "Mechanical", "location trigger", "manual reset", "BA +8 ranged, 1d4 dart damage plus poison", "poison: bloodroot, DC 12 Fortitude save, no primary damage, secondary is 1d4 Constitution and 1d3 Wisdom")]
        [TestCase(17, 19, "Poisoned needle trap", 1, 22, 20, "Mechanical", "touch trigger", "manual reset", "BA +8 ranged, 1 needle damage plus poison", "poison: greenblood oil, DC 13 Fortitude save, primary is 1 Constitution, secondary is 1d2 Constitution")]
        [TestCase(20, 22, "Portcullis trap", 1, 20, 20, "Mechanical", "location trigger", "manual reset", "BA +10 melee, 3d6 portcullis damage, only those underneath portcullis", "blocks passageway once triggered")]
        [TestCase(23, 24, "Razor-wire across area", 1, 22, 15, "Mechanical", "location trigger", "no reset", "BA +10 melee, 2d6 wire damage", "multiple targets: first 2 within 5'")]
        [TestCase(25, 27, "Rolling rock trap", 1, 20, 22, "Mechanical", "location trigger", "manual reset", "BA +10 melee, 2d6 rock damage")]
        [TestCase(28, 30, "Scything blade trap", 1, 21, 20, "Mechanical", "location trigger", "manual reset", "BA +8 melee, 1d8 scythe damage, crit x3")]
        [TestCase(31, 33, "Spear trap", 1, 20, 20, "Mechanical", "location trigger", "manual reset", "BA +12 ranged, 1d8 spear damage, crit x3", "200' max range", "target determined randomly from those in its path")]
        [TestCase(34, 35, "Swinging block trap", 1, 20, 20, "Mechanical", "touch trigger", "manual reset", "BA +5 melee, 4d6 block damage")]
        [TestCase(36, 38, "Wall blade trap", 1, 22, 22, "Mechanical", "touch trigger", "manual reset", "hidden switch bypass: Search DC 25", "BA +10 melee, 2d4 scythe damage, crit x4")]
        [TestCase(39, 41, "Box of brown mold", 2, 22, 16, "Mechanical", "touch trigger, opening the box", "automatic reset", "5' cold aura, 3d6 cold damage, nonlethal")]
        [TestCase(42, 44, "Bricks from ceiling", 2, 20, 20, "Mechanical", "touch trigger", "repair reset", "BA +12 melee, 2d6 brick damage", "multiple targets: all within 5'")]
        [TestCase(45, 47, "Burning hands trap", 2, 26, 26, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: burning hands, 1st-level wizard, 1d4 fire damage, DC 11 Reflex save for half damage")]
        [TestCase(48, 50, "Camouflaged pit trap", 2, 24, 19, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "20' deep, 2d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(51, 53, "Inflict light wounds trap", 2, 26, 26, "Magic device", "touch trigger", "automatic reset", "spell effect: inflict light wounds, 1st-level cleric, 1d8+1 damage, DC 11 Will save for half damage")]
        [TestCase(54, 56, "Javelin trap", 2, 20, 18, "Mechanical", "location trigger", "manual reset", "BA +16 ranged, 1d6+4 javelin damage")]
        [TestCase(57, 58, "Large net trap", 2, 20, 25, "Mechanical", "location trigger", "manual reset", "BA +5 melee, characters in 10' square are grappled by net if they fail a DC 14 Reflex save", "net has grapple Strength of 18")]
        [TestCase(59, 61, "Pit trap", 2, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "40' deep, 4d6 fall damage")]
        [TestCase(62, 64, "Poison needle trap", 2, 22, 17, "Mechanical", "touch trigger", "repair reset", "lock bypass: Open Lock DC 30/BA +17 melee, 1 needle damage plus poison/poison: blue whinnis, DC 14 Fortitude save, primary damage is 1 Constitution, secondary damage is unconsciousness")]
        [TestCase(65, 67, "Spiked pit trap", 2, 18, 15, "Mechanical", "location trigger", "automatic reset", "DC 20 Reflex save avoids", "20' deep, 2d6 fall damage", "multiple targets: first 2 within 5'", "pit spikes: BA +10 melee, 1d4 spikes per target, 1d4+2 damage each")]
        [TestCase(68, 69, "Tripping chain", 2, 15, 18, "Mechanical", "location trigger", "automatic reset", "multiple traps", "trip: BA +15 melee touch", "spiked chain: BA +15 melee, 2d4+2 damage", "If trip attack succeeds, spiked chain is +4 to hit because target is prone")]
        [TestCase(70, 72, "Well-camouflaged pit trap", 2, 27, 20, "Mechanical", "location trigger", "repair reset", "DC 20 Reflex save avoids", "10' deep, 1d6 fall damage")]
        [TestCase(73, 75, "Burning hands trap", 3, 26, 26, "Magic device", "proximity trigger, alarm spell", "automatic reset", "spell effect: burning hands, 5th-level wizard, 5d4 fire damage, DC 11 Reflex save for half damage")]
        [TestCase(76, 78, "Camouflaged pit trap", 3, 24, 18, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "30' deep, 3d6 fall damage", "multiple targets: first 2 within 5'")]
        [TestCase(79, 80, "Ceiling pendulum", 3, 15, 27, "Mechanical", "location trigger", "automatic reset", "BA +15 melee, 1d12 greataxe damage, crit x3")]
        [TestCase(81, 83, "Fire trap", 3, 27, 27, "Spell", "spell trigger, see spell description", "no reset", "spell effect: fire trap, 3rd-level druid, 1d4+3 fire damage, DC 13 Reflex save for half damage")]
        [TestCase(84, 85, "Extended bane trap", 3, 27, 27, "Magic device", "proximity trigger, detect good spell", "automatic reset", "spell effect: extended bane, 3rd-level cleric, DC 13 Will save negates")]
        [TestCase(86, 87, "Ghoul touch trap", 3, 27, 27, "Magic device", "touch trigger", "automatic reset", "spell effect: ghoul touch, 3rd-level wizard, DC 13 Fortitude save negates")]
        [TestCase(88, 90, "Hail of needles", 3, 22, 22, "Mechanical", "location trigger", "manual reset", "BA +20 ranged, 2d4 needle damage")]
        [TestCase(91, 92, "Melf's acid arrow trap", 3, 27, 27, "Magic device", "proximity trigger, alarm spell", "automatic reset", "BA +2 ranged, touch", "spell effect: melf's acid arrow, 3rd-level wizard, 2d4 acid per round for 2 rounds")]
        [TestCase(93, 95, "Pit trap", 3, 20, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "60' deep, 6d6 fall damage")]
        [TestCase(96, 98, "Spiked pit trap", 3, 21, 20, "Mechanical", "location trigger", "manual reset", "DC 20 Reflex save avoids", "20' deep, 2d6 fall damage", "multiple targets: first 2 within 5'", "pit spikes: BA +10, 1d4 spikes per target, 1d4+2 damage each")]
        [TestCase(99, 100, "Stone blocks from ceiling", 3, 25, 20, "Mechanical", "location trigger", "repair reset", "BA +10 melee, 4d6 stone block damage")]
        public void Trap(int lower, int upper, string name, int challengeRating, int searchDC, int disableDeviceDC, params string[] descriptions)
        {
            var description = string.Join("/", descriptions);
            base.AreaPercentile(lower, upper, name, description, challengeRating.ToString(), searchDC, disableDeviceDC);
        }
    }
}
