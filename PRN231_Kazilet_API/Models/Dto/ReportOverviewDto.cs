namespace PRN231_Kazilet_API.Models.Dto
{
    public class ReportOverviewDto
    {
        public int Rank { get; set; }
        public string Username { get; set; }
        public float Duration { get; set; }

        public int Score { get; set; }

        public ReportOverviewDto(int rank, string username, float duration, int score)
        {
            Rank = rank;
            Username = username;
            Duration = duration;
            Score = score;
        }
    }
}
