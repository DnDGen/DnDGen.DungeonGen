using EncounterGen.Common;
using System.Collections.Generic;
using System.Linq;
using TreasureGen.Common;

namespace DungeonGen.Common
{
    public class Contents
    {
        public IEnumerable<Encounter> Encounters { get; set; }
        public Treasure Treasure { get; set; }
        public string TreasureContainer { get; set; }
        public IEnumerable<string> Miscellaneous { get; set; }
        public IEnumerable<Trap> Traps { get; set; }

        public bool IsEmpty
        {
            get
            {
                return !Encounters.Any() && string.IsNullOrEmpty(TreasureContainer) && !Miscellaneous.Any() && !Traps.Any()
                    && Treasure.Coin.Quantity == 0 && !Treasure.Goods.Any() && !Treasure.Items.Any();
            }
        }

        public Contents()
        {
            Treasure = new Treasure();
            Encounters = Enumerable.Empty<Encounter>();
            TreasureContainer = string.Empty;
            Miscellaneous = Enumerable.Empty<string>();
            Traps = Enumerable.Empty<Trap>();
        }
    }
}
