using TreasureGen.Common;

namespace DungeonGen.Common
{
    public class ContainedTreasure
    {
        public string Container { get; set; }
        public Treasure Treasure { get; set; }

        public ContainedTreasure()
        {
            Container = string.Empty;
            Treasure = new Treasure();
        }
    }
}
