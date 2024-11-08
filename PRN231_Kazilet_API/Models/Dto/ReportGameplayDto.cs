namespace PRN231_Kazilet_API.Models.Dto
{
    public class ReportGameplayDto
    {
        public string Code { get; set; }    
        public float CorrectPercent { get;set; }

        public float IncorrectPercent { get;set; }

        public int NoPlayers { get; set; }

        public int NoQuestions { get; set; }

        public List<ReportOverviewDto> Overview { get; set; }

        public List<ReportQuestionDto> Question { get; set; }
    }
}
