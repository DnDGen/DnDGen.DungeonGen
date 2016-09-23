using System.Collections.Generic;
using System.Linq;

namespace DungeonGen
{
    public class Trap
    {
        public IEnumerable<string> Descriptions { get; set; }
        public int ChallengeRating { get; set; }
        public int SearchDC { get; set; }
        public int DisableDeviceDC { get; set; }
        public string Name { get; set; }

        public Trap()
        {
            Descriptions = Enumerable.Empty<string>();
            Name = string.Empty;
        }
    }
}
