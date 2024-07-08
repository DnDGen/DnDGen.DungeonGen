using DnDGen.EncounterGen.Models;

namespace DnDGen.DungeonGen.Models
{
    public class Pool
    {
        public Encounter Encounter { get; set; }
        public DungeonTreasure Treasure { get; set; }
        public string MagicPower { get; set; }

        public Pool()
        {
            MagicPower = string.Empty;
        }
    }
}
