namespace PRN231_Kazilet_API.Models.Dto
{
    public class PlayerDto
    {
        public string Username { get; set; }

        public int Score { get; set; }

        public PlayerDto(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }
}
