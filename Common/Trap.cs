namespace DungeonGen.Common
{
    public class Trap
    {
        public string Description { get; set; }
        public int ChallengeRating { get; set; }
        public int DieCheck { get; set; }

        public Trap()
        {
            Description = string.Empty;
        }
    }
}
