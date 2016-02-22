using EncounterGen.Common;
using System.Collections.Generic;
using System.Linq;
using TreasureGen.Common;

namespace DungeonGen.Common
{
    public class Contents
    {
        public Encounter Encounter { get; set; }
        public Treasure Treasure { get; set; }
        public string TreasureContainer { get; set; }
        public IEnumerable<string> Miscellaneous { get; set; }

        public Contents()
        {
            Treasure = new Treasure();
            Encounter = new Encounter();
            TreasureContainer = string.Empty;
            Miscellaneous = Enumerable.Empty<string>();
        }
    }
}
