using EncounterGen.Common;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGen
{
    public class Contents
    {
        public IEnumerable<Encounter> Encounters { get; set; }
        public IEnumerable<DungeonTreasure> Treasures { get; set; }
        public IEnumerable<string> Miscellaneous { get; set; }
        public IEnumerable<Trap> Traps { get; set; }
        public Pool Pool { get; set; }

        public bool IsEmpty
        {
            get
            {
                return !Encounters.Any() && !Miscellaneous.Any() && !Traps.Any() && !Treasures.Any() && Pool == null;
            }
        }

        public Contents()
        {
            Treasures = Enumerable.Empty<DungeonTreasure>();
            Encounters = Enumerable.Empty<Encounter>();
            Miscellaneous = Enumerable.Empty<string>();
            Traps = Enumerable.Empty<Trap>();
        }
    }
}
