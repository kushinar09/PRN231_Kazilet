using Microsoft.OData.Edm;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;
using PRN231_Kazilet_API.Utils;
using System.Security.Cryptography;

namespace PRN231_Kazilet_API.Services.Impl
{
    public class GameplayService : IGameplayService
    {
        private readonly PRN231_KaziletContext _context;

        private readonly IAuthService _authService;

        private readonly IQuestionService _questionService;

        public GameplayService(PRN231_KaziletContext context, IAuthService authService, IQuestionService questionService)
        {
            _context = context;
            _authService = authService;
            _questionService = questionService;
        }

        public string HostGame(int courseId, string username)
        {
            User user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                string code;
                code = GameplayUtils.GenerateUniqueRandomNumbers();
                while (CheckExistCode(code))
                {
                    code = GameplayUtils.GenerateUniqueRandomNumbers();
                }
                GameplaySetting gameplaySetting = new GameplaySetting(code, DateTime.Now, user.Id);
                _context.GameplaySettings.Add(gameplaySetting);
                Gameplay gameplay = new Gameplay(code, username, 0);
                _context.Gameplays.Add(gameplay);
                _context.SaveChanges();
                return code;
            }
            else
            {
                return "";
            }
        }

        public string JoinGame(string code, string username)
        {
            if(CheckExistCode(code))
            {
                if (_context.Gameplays.FirstOrDefault(g => g.Code == code && g.Username == username) == null)
                {
                    string token = _authService.GetGameplayToken(code, username);
                    Gameplay gameplay = new Gameplay(code, username, 0);
                    _context.Gameplays.Add(gameplay);
                    _context.SaveChanges();
                    return token;
                }
            }
            return "";
        }

        public bool CheckExistCode(string code)
        {
            if(_context.GameplaySettings.FirstOrDefault(g => (g.IsCompleted == false || g.IsCompleted == null) && g.Code == code) != null)
            {
                return true;
            }
            return false;
        }

        public List<string> GetPlayerInRoom(string code)
        {
            List<string> list = _context.Gameplays.Where(g => g.Code == code).Select(g => g.Username).ToList();
            return list;
        }

        public int[] GetQuestionAlreadyAnswer(string code)
        {
            int[] questions = (_context.Gameplays.Where(g => g.Turn != 0 && g.Code == code).Where(q => q.QuestionId.HasValue).Select(g => g.QuestionId.Value).ToArray());
            return questions;
        }

    }
}
