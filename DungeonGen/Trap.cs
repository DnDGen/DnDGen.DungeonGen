namespace DungeonGen
{
    public class Trap
    {
        public string Description { get; set; }
        public int ChallengeRating { get; set; }
        public int SearchDC { get; set; }
        public int DisableDeviceDC { get; set; }

        public Trap()
        {
            Description = string.Empty;
        }
    }
}
