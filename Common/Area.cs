namespace DungeonGen.Common
{
    public class Area
    {
        public string Type { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public string Description { get; set; }
        public Contents Contents { get; set; }

        public Area()
        {
            Contents = new Contents();
            Description = string.Empty;
            Type = string.Empty;
        }
    }
}
