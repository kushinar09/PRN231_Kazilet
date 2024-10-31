using Microsoft.EntityFrameworkCore;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services
{
    public interface IGameplayService
    {
        public string HostGame(int courseId, string username, HttpContext httpContext);

        public Task StartGame(string code, string username);

        public GameplaySettingDto UpdateGameplaySetting(GameplaySettingDto gameplaySettingDto);

        public bool CheckExistCode(string code);

        public string JoinGame(string code, string username, HttpContext httpContext);

        public List<string> GetPlayerInRoom(string code);

        public int[] GetQuestionAlreadyAnswer(string code);

        public int AddPlayerAnswer(string code, string username, PlayerAnswerDto playerAnswerDto, HttpContext httpContext);

        public List<GameplayResultDto> GetGameplayResultForTurn(string code, int turn);

        public List<GameplayReportDto> GetGameplayReportForTurn(string code, int turn);

        public GameplayRankingDto GetGameplayRankingForTurn(string code, int turn);

        public GameplaySettingDto GetGameplaySettingDtoByCode(string code);

        public GameplayFinalReportDto GetGameplayFinalReport(string code, string username);

        public int GetCurrentQuestion(string code);

        public int GetTotalQuestions(string code);

        public int GetAnswerStreak(string code, string username);

        public int GetTimeLimit(string code);

        public int GetPoint(string code, string username);

        public int GetLatestTurn(string code);

        public void DeleteFromLobby(string code, string username);

        public void UpdateGetResult(string code, int turn);

        public PlayerAnswerDto CheckSubmittedLatest(string code, string username);
    }
}
