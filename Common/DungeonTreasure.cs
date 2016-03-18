using TreasureGen.Common;

namespace DungeonGen.Common
{
    public class DungeonTreasure
    {
        public string Container { get; set; }
        public Treasure Treasure { get; set; }
        public string Concealment { get; set; }

        public DungeonTreasure()
        {
            Container = string.Empty;
            Treasure = new Treasure();
            Concealment = string.Empty;
        }
    }
}
