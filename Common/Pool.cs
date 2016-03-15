using EncounterGen.Common;

namespace DungeonGen.Common
{
    public class Pool
    {
        public Encounter Encounter { get; set; }
        public ContainedTreasure Treasure { get; set; }
        public string MagicPower { get; set; }

        public Pool()
        {
            MagicPower = string.Empty;
        }
    }
}
