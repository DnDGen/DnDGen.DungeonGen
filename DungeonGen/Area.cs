using System.Collections.Generic;
using System.Linq;

namespace DungeonGen
{
    public class Area
    {
        public string Type { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public IEnumerable<string> Descriptions { get; set; }
        public Contents Contents { get; set; }

        public Area()
        {
            Contents = new Contents();
            Descriptions = Enumerable.Empty<string>();
            Type = string.Empty;
        }
    }
}
