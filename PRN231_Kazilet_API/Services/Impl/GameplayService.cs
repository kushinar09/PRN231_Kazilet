//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.OData.Edm;
//using Newtonsoft.Json;
//using PRN231_Kazilet_API.Models.Dto;
//using PRN231_Kazilet_API.Models.Entities;
//using PRN231_Kazilet_API.Utils;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Cryptography;
//using System.Text.Json.Serialization;
//using static System.Net.WebRequestMethods;

//namespace PRN231_Kazilet_API.Services.Impl
//{
//    public class GameplayService : IGameplayService
//    {
//        private readonly PRN231_Kazilet_v2Context _context;

//        private readonly IAuthService _authService;

//        private readonly IQuestionService _questionService;

//        private readonly IMapper _mapper;

//        private readonly IHubContext<SignalrServer> _signalRHub;

//        private int baseScore = 500;

//        public GameplayService(PRN231_Kazilet_v2Context context, IAuthService authService, IQuestionService questionService, IMapper mapper, IHubContext<SignalrServer> signalRHub)
//        {
//            _context = context;
//            _authService = authService;
//            _questionService = questionService;
//            _mapper = mapper;
//            _signalRHub = signalRHub;
//        }

//        public string HostGame(int courseId, string username, HttpContext httpContext)
//        {
//            /*
//            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
//            // Check if it starts with "Bearer "
//            if (authHeader.StartsWith("Bearer "))
//            {
//                // Extract token string
//                var token = authHeader.Substring("Bearer ".Length).Trim();
//                if (username != null)
//                {
//                    User user = _authService.GetUserFromJwtToken(token);
//                    string code;
//                    code = GameplayUtils.GenerateUniqueRandomNumbers();
//                    while (CheckExistCode(code))
//                    {
//                        code = GameplayUtils.GenerateUniqueRandomNumbers();
//                    }
//                    GameplaySetting gameplaySetting = new GameplaySetting(code, DateTime.Now, user.Id);
//                    gameplaySetting.CreatedBy = user.Id;
//                    gameplaySetting.TimeLimit = 15;
//                    gameplaySetting.CourseId = courseId;
//                    gameplaySetting.IsSkillEnabled = true;
//                    gameplaySetting.NoQuestion = 20;
//                    _context.GameplaySettings.Add(gameplaySetting);
//                    Gameplay gameplay = new Gameplay(code, username, 0);

//                    _context.Gameplays.Add(gameplay);
//                    _context.SaveChanges();
//                    return code;
//                }
//                else
//                {
//                    return "";
//                }
//            }
//            Console.WriteLine("HIHI" + "AHIHO");
//            return "";
//            */
//            if (username != null)
//            {
//                string code;
//                code = GameplayUtils.GenerateUniqueRandomNumbers();
//                while (CheckExistCode(code))
//                {
//                    code = GameplayUtils.GenerateUniqueRandomNumbers();
//                }
//                GameplaySetting gameplaySetting = new GameplaySetting(code, DateTime.Now, 1);
//                gameplaySetting.CreatedBy = 1;
//                gameplaySetting.TimeLimit = 15;
//                gameplaySetting.CourseId = courseId;
//                gameplaySetting.IsSkillEnabled = true;
//                gameplaySetting.NoQuestion = 20;
//                _context.GameplaySettings.Add(gameplaySetting);
//                Gameplay gameplay = new Gameplay(code, username, 0);

//                _context.Gameplays.Add(gameplay);
//                _context.SaveChanges();
//                return code;
//            }
//            else
//            {
//                return "";
//            }

//        }

//        public int GetCurrentQuestion(string code)
//        {
//            return (int)_context.Gameplays.Where(g => g.Code == code).Max(g => g.Turn) + 1;
//        }

//        public int GetTotalQuestions(string code)
//        {
//            return (int)_context.GameplaySettings.FirstOrDefault(gs => gs.Code == code).NoQuestion;
//        }

//        public int GetAnswerStreak(string code, string username)
//        {
//            var lastId = _context.Gameplays.Where(g => g.Code == code && g.Username == username).Max(g => g.Id);
//            Console.WriteLine("Last Streak: " + lastId);
//            return _context.Gameplays.FirstOrDefault(g => g.Id == lastId).Streak != null ? (int)_context.Gameplays.FirstOrDefault(g => g.Id == lastId).Streak : 0;
//        }

//        private int GetPlayerAnswerStreak(string code, string username)
//        {
//            var lastId = _context.Gameplays.Where(g => g.Code == code && g.Username == username).Max(g => g.Id);
//            return _context.Gameplays.FirstOrDefault(g => g.Id == lastId).Streak != null ? (int)_context.Gameplays.FirstOrDefault(g => g.Id == lastId).Streak : 0;
//        }

//        public int GetTimeLimit(string code)
//        {
//            return (int)_context.GameplaySettings.FirstOrDefault(gs => gs.Code == code).TimeLimit;
//        }

//        public int GetPoint(string code, string username)
//        {
//            Console.WriteLine("Point: " + _context.Gameplays
//                .Where(g => g.Code == code && g.Username == username)
//                .Sum(g => g.Score.GetValueOrDefault(0)) + " " + username + " " + code);
//            return _context.Gameplays
//                .Where(g => g.Code == code && g.Username == username)
//                .Sum(g => g.Score.GetValueOrDefault(0));
//        }

//        public string JoinGame(string code, string username, HttpContext httpContext)
//        {
//            if (CheckExistCode(code))
//            {
//                if (_context.Gameplays.FirstOrDefault(g => g.Code == code && g.Username == username) == null)
//                {

//                    // Extract token string

//                    string gameToken = _authService.GetGameplayToken(code, username);
//                    Gameplay gameplay = new Gameplay(code, username, 0);
//                    _context.Gameplays.Add(gameplay);
//                    _context.SaveChanges();
//                    return gameToken;

//                }
//            }
//            return "";
//        }

//        public bool CheckExistCode(string code)
//        {
//            if (_context.GameplaySettings.FirstOrDefault(g => (g.IsCompleted == false || g.IsCompleted == null) && (g.IsStarted == false || g.IsStarted == null) && g.Code == code) != null)
//            {
//                return true;
//            }
//            return false;
//        }

//        public List<string> GetPlayerInRoom(string code)
//        {
//            List<string> list = _context.Gameplays.Where(g => g.Code == code && g.Turn == 0).Select(g => g.Username).ToList();
//            return list;
//        }

//        public int[] GetQuestionAlreadyAnswer(string code)
//        {
//            //int[] questions = (_context.Gameplays.Where(g => g.Turn != 0 && g.Code == code).Where(q => q.QuestionId.HasValue).Select(g => g.QuestionId.Value).ToArray());
//            return questions;
//        }

//        public GameplaySettingDto UpdateGameplaySetting(GameplaySettingDto gameplaySettingDto)
//        {
//            GameplaySetting gameplaySetting = _context.GameplaySettings.FirstOrDefault(g => g.Code == gameplaySettingDto.Code);
//            gameplaySetting.TimeLimit = gameplaySettingDto.TimeLimit;
//            gameplaySetting.IsSkillEnabled = gameplaySettingDto.IsSkillEnabled;
//            gameplaySetting.NoQuestion = gameplaySettingDto.NoQuestion;
//            _context.GameplaySettings.Update(gameplaySetting);
//            _context.SaveChanges();
//            return _mapper.Map<GameplaySettingDto>(gameplaySetting);
//        }

//        public async Task StartGame(string code, string username)
//        {
//            int courseId = (int)_context.GameplaySettings.FirstOrDefault(c => c.Code == code).CourseId;
//            List<QuestionDto> questionDtos = _questionService.GetAllQuestionsByCourse(courseId);
//            int questionId = GameplayUtils.GenerateRandom(questionDtos.Count);
//            while (GetQuestionAlreadyAnswer(code).Contains(questionId))
//            {
//                questionId = GameplayUtils.GenerateRandom(questionDtos.Count);
//            }
//            GameplaySetting gameplaySetting = _context.GameplaySettings.FirstOrDefault(gs => gs.Code == code);
//            gameplaySetting.IsStarted = true;
//            _context.SaveChanges();
//            QuestionDto questionDto = _questionService.GetById(questionId);
//            List<GameplayAddI> gameplayAdds = new List<GameplayAddI>();
//            List<string> players = GetPlayerInRoom(code);
//            for (int i = 0; i < players.Count; i++)
//            {
//                int currentQuestion = GetCurrentQuestion(code);
//                int totalQuestion = GetTotalQuestions(code);
//                int streak = GetAnswerStreak(code, players[i]);
//                int timeLimit = GetTimeLimit(code);
//                int point = GetPoint(code, players[i]);
//                gameplayAdds.Add(new GameplayAddI(players[i], currentQuestion, totalQuestion, streak, timeLimit, point, DateTime.Now));
                
//            }
//            await Console.Out.WriteLineAsync("N: " + questionDto.Answers.ToList().Count);
//            for (int i = 0; i < questionDto.Answers.ToList().Count; i++)
//            {
//                await Console.Out.WriteLineAsync("N" + questionDto.Answers.ToList()[i].Content);
//            }
//            GameplayQuestionDto gameplayQuestionDto = new GameplayQuestionDto(questionDto, gameplayAdds);
//            //await _signalRHub.Clients.Group(code).SendAsync("GetQuestion", questionDto);
//            string json = JsonConvert.SerializeObject(gameplayQuestionDto);
//            await _signalRHub.Clients.Group(code).SendAsync("GetQuestion", json);

//        }

//        //public int AddPlayerAnswer(string code, string username, PlayerAnswerDto playerAnswerDto, HttpContext httpContext)
//        //{
//        //    var authHeader = httpContext.Request.Headers["Authorization"].ToString();
//        //    User user;
//        //    // Check if it starts with "Bearer "

//        //    GameplaySetting gameplaySetting = _context.GameplaySettings.FirstOrDefault(gs => gs.Code == code);
//        //    Gameplay gameplay = new Gameplay();

           
//        //    gameplay.Code = code;
//        //    gameplay.Username = username;
//        //    gameplay.QuestionId = playerAnswerDto.QuestionId;
//        //    if (playerAnswerDto.PlayerAnswer == 0)
//        //        gameplay.PlayerAnswer = null;
//        //    else
//        //        gameplay.PlayerAnswer = playerAnswerDto.PlayerAnswer;
//        //    gameplay.Turn = playerAnswerDto.Turn;
//        //    gameplay.CreatedAt = playerAnswerDto.CreatedAt;
//        //    int duration = (int)Math.Floor((playerAnswerDto.AnswerAt - playerAnswerDto.CreatedAt).TotalSeconds);
//        //    gameplay.Duration = duration;
//        //    bool isCorrect;
//        //    isCorrect = false;
//        //    if (playerAnswerDto.PlayerAnswer != null)
//        //        if (_context.Answers.FirstOrDefault(a => a.Id == playerAnswerDto.PlayerAnswer) != null)
//        //            isCorrect = (bool)_context.Answers.FirstOrDefault(a => a.Id == playerAnswerDto.PlayerAnswer).IsCorrect;
//        //    gameplay.IsCorrect = isCorrect;
//        //    int streak = GetPlayerAnswerStreak(code, username);
//        //    if (isCorrect)
//        //    {
//        //        streak++;
//        //    }
//        //    else
//        //    {
//        //        streak = 0;
//        //    }
//        //    gameplay.Streak = streak;
//        //    if (gameplay.IsCorrect == true)
//        //        gameplay.Score = calculateScore(streak, duration, (int)gameplaySetting.TimeLimit, playerAnswerDto.Turn, code);
//        //    else
//        //        gameplay.Score = 0;
//        //    _context.Gameplays.Add(gameplay);
//        //    _context.SaveChanges();
//        //    int numberSubmitted = _context.Gameplays.Where(g => g.Code == code && g.Turn == playerAnswerDto.Turn).Count();
//        //    return numberSubmitted;
//        //}

//        private int calculateScore(int streak, int duration, int timeLimit, int turn, string code)
//        {
//            int score = baseScore;
//            if (streak <= 2)
//            {
//                score += 50;
//            }
//            else if (streak <= 6)
//            {
//                score += 100;
//            }
//            else if (streak <= 9)
//            {
//                score += 150;
//            }
//            else
//            {
//                score += 250;
//            }


//            if (duration <= Math.Ceiling(timeLimit * 0.25))
//            {
//                score += 200;
//            }
//            else if (duration <= Math.Ceiling(timeLimit * 0.5))
//            {
//                score += 100;
//            }
//            else if (duration <= Math.Ceiling(timeLimit * 0.75))
//            {
//                score += 50;
//            }

//            int numberSubmitted = _context.Gameplays.Where(g => g.Code == code && g.Turn == turn).Count();
//            if (numberSubmitted == 0)
//            {
//                score += 200;
//            }
//            else if (numberSubmitted == 1)
//            {
//                score += 150;
//            }
//            else if (numberSubmitted == 2)
//            {
//                score += 100;
//            }
//            else if (numberSubmitted <= 5)
//            {
//                score += 75;
//            }
//            else if (numberSubmitted <= 10)
//            {
//                score += 50;
//            }
//            else if (numberSubmitted <= 20)
//            {
//                score += 25;
//            }
//            return score;
//        }

//        public List<GameplayResultDto> GetGameplayResultForTurn(string code, int turn)
//        {
//            var gameplays = _context.Gameplays.Where(g => g.Code == code && g.Turn != 0)
//                .GroupBy(g => g.Username)
//                .Select(gm => new
//                {
//                    Score = gm.Sum(g => g.Score),
//                    Duration = gm.Average(g => g.Duration),
//                    Username = gm.Key
//                })
//                .OrderByDescending(g => g.Score)
//                .ThenBy(g => g.Duration)
//                .ToList();
//            List<GameplayResultDto> gameplayResultDtos = new List<GameplayResultDto>();
//            for (int i = 0; i < gameplays.Count; i++)
//            {
//                GameplayResultDto gameplayResultDto = new GameplayResultDto();
//                gameplayResultDto.Username = gameplays[i].Username;
//                //gameplayResultDto.IsCorrect = (bool)_context.Gameplays.FirstOrDefault(g => g.Username == gameplays[i].Username && g.Code == code && g.Turn == turn).IsCorrect;
//                gameplayResultDto.Streak = (int)_context.Gameplays.FirstOrDefault(g => g.Username == gameplays[i].Username && g.Code == code && g.Turn == turn).Streak;
//                gameplayResultDto.Score = (int)_context.Gameplays.FirstOrDefault(g => g.Username == gameplays[i].Username && g.Code == code && g.Turn == turn).Score;
//                gameplayResultDto.Place = i + 1;
//                gameplayResultDtos.Add(gameplayResultDto);
//            }
//            return gameplayResultDtos;
//        }

//        //public List<GameplayReportDto> GetGameplayReportForTurn(string code, int turn)
//        //{
//        //    var gameplays = _context.Gameplays.Where(g => g.Code == code && g.Turn == turn)
//        //        .GroupBy(g => g.PlayerAnswer)
//        //        .Select(gm => new
//        //        {
//        //            No = gm.Count(),
//        //            AnswerId = gm.Key
//        //        })
//        //        .ToList();
//        //    List<GameplayReportDto> gameplayReportDtos = new List<GameplayReportDto>();
//        //    int questionId = (int)_context.Gameplays.FirstOrDefault(g => g.Code == code && g.Turn == turn).QuestionId;
//        //    List<Answer> answers = _context.Answers.Where(a => a.QuestionId == questionId).ToList();
//        //    for (int i = 0; i < answers.Count; i++)
//        //    {
//        //        GameplayReportDto gameplayReportDto = new GameplayReportDto();
//        //        gameplayReportDto.AnswerId = (int)answers[i].Id;
//        //        bool flag = false;
//        //        for (int j = 0; j < gameplays.Count; j++)
//        //        {
//        //            if (gameplays[j].AnswerId != null && (int)gameplays[j].AnswerId == answers[i].Id)
//        //            {
//        //                flag = true;
//        //                gameplayReportDto.No = (int)gameplays[j].No;
//        //            }
//        //        }
//        //        if (!flag)
//        //            gameplayReportDto.No = 0;
//        //        gameplayReportDto.IsCorrect = (bool)answers[i].IsCorrect;
//        //        gameplayReportDtos.Add(gameplayReportDto);
//        //    }
//        //    return gameplayReportDtos;
//        //}

//        public GameplayRankingDto GetGameplayRankingForTurn(string code, int turn)
//        {
//            var lastTurn = _context.Gameplays.Where(g => g.Code == code && g.Turn < turn)
//                .GroupBy(g => g.Username)
//                .Select(gm => new
//                {
//                    Username = gm.Key,
//                    Duration = gm.Average(gm => gm.Duration),
//                    Score = gm.Sum(gm => gm.Score)
//                })
//                .OrderByDescending(gm => gm.Score)
//                .ThenBy(gm => gm.Duration)
//                .ThenBy(gm => gm.Username)
//                .ToList();
//            List<PlayerDto> oldRank = new List<PlayerDto>();
//            for (int i = 0; i < lastTurn.Count; i++)
//            {
//                oldRank.Add(new PlayerDto(lastTurn[i].Username, (int)lastTurn[i].Score));
//            }

//            List<PlayerDto> newRank = new List<PlayerDto>();
//            for (int i = 0; i < oldRank.Count; i++)
//            {
//                newRank.Add(new PlayerDto(oldRank[i].Username, oldRank[i].Score + (int)_context.Gameplays.FirstOrDefault(g => g.Username == oldRank[i].Username && g.Code == code && g.Turn == turn).Score));
//            }
//            GameplayRankingDto gameplayRankingDto = new GameplayRankingDto(oldRank, newRank);
//            return gameplayRankingDto;
//        }

//        public GameplaySettingDto GetGameplaySettingDtoByCode(string code)
//        {
//            GameplaySetting gameplaySetting = _context.GameplaySettings.FirstOrDefault(gs => gs.Code == code);
//            if (gameplaySetting != null)
//            {
//                return _mapper.Map<GameplaySettingDto>(gameplaySetting);
//            }
//            return null;
//        }

//        public int GetNumberCorrectAnswer(string code, string username)
//        {
//            //return _context.Gameplays.Where(g => g.Code == code && g.Username == username && g.IsCorrect == true && g.Turn != 0).Count();
//        }

//        public double GetAvgDuration(string code, string username)
//        {
//            return (double)_context.Gameplays.Where(g => g.Code == code && g.Username == username && g.Turn != 0).Average(g => g.Duration);
//        }

//        public int GetHighestStreak(string code, string username)
//        {
//            return (int)_context.Gameplays.Where(g => g.Code == code && g.Username == username && g.Turn != 0).Max(g => g.Streak);
//        }

//        public int GetTotalScore(string code, string username)
//        {
//            return _context.Gameplays.Where(g => g.Code == code && g.Username == username && g.Turn != 0).Sum(g => g.Score.GetValueOrDefault(0));
//        }

//        public int GetPlace(string code, string username)
//        {
//            var list = _context.Gameplays.Where(g => g.Code == code && g.Turn != 0)
//                .GroupBy(g => g.Username)
//                .Select(gm => new
//                {
//                    Username = gm.Key,
//                    Score = gm.Sum(gm => gm.Score),
//                    Duration = gm.Average(gm => gm.Duration)
//                })
//                .OrderByDescending(gm => gm.Score)
//                .ThenBy(gm => gm.Duration)
//                .ThenBy(gm => gm.Username)
//                .ToList();
//            for (int i = 0; i < list.Count; i++)
//            {
//                if (username == list[i].Username)
//                {
//                    return i + 1;
//                }
//            }
//            return 0;
//        }

//        public int GetTotalPlayers(string code)
//        {
//            return _context.Gameplays
//                .Where(g => g.Code == code)
//                .Select(g => g.Username)
//                .Distinct()
//                .Count();
//        }

//        public GameplayFinalReportDto GetGameplayFinalReport(string code, string username)
//        {
//            GameplayFinalReportDto gameplayFinalReportDto = new GameplayFinalReportDto();
//            gameplayFinalReportDto.Code = code;
//            gameplayFinalReportDto.Username = username;
//            gameplayFinalReportDto.Place = GetPlace(code, username);
//            gameplayFinalReportDto.TotalPlayers = GetTotalPlayers(code);
//            gameplayFinalReportDto.CorrectAnswer = GetNumberCorrectAnswer(code, username);
//            gameplayFinalReportDto.TotalQuestion = (int)_context.GameplaySettings.FirstOrDefault(gs => gs.Code == code).NoQuestion;
//            gameplayFinalReportDto.Duration = GetAvgDuration(code, username);
//            gameplayFinalReportDto.Score = GetTotalScore(code, username);
//            gameplayFinalReportDto.HighestStreak = GetHighestStreak(code, username);
//            List<PlayerResponseDtocs> playerResponseDtocs = new List<PlayerResponseDtocs>();
//            List<Gameplay> gameplays = _context.Gameplays.Where(g => g.Code == code && g.Username == username && g.Turn != 0).ToList();
//            for (int i = 0; i < gameplays.Count; i++)
//            {
//                PlayerResponseDtocs playerResponse = new PlayerResponseDtocs();
//                //playerResponse.PlayerAnswer = (int)gameplays[i].PlayerAnswer.GetValueOrDefault(0);
//                playerResponse.QuestionDto = _mapper.Map<QuestionDto>(_context.Questions
//                    .Include(q => q.Answers)
//                    //.FirstOrDefault(q => q.Id == gameplays[i].QuestionId));
//                //playerResponseDtocs.Add(playerResponse);
//            }
//            gameplayFinalReportDto.PlayerResponses = playerResponseDtocs;
//            return gameplayFinalReportDto;
//        }

//        public int GetLatestTurn(string code)
//        {
//            var lastId = _context.Gameplays.Where(g => g.Code == code).Max(g => g.Id);
//            return _context.Gameplays.FirstOrDefault(g => g.Id == lastId).Turn.GetValueOrDefault(0);
//        }

//        public void DeleteFromLobby(string code, string username)
//        {
//            _context.Gameplays
//                .Where(g => g.Code == code && g.Username == username)
//                .ToList()
//                .ForEach(g => _context.Remove(g));
//            _context.SaveChanges();
//        }

//        public void UpdateGetResult(string code, int turn)
//        {
//            _context.Gameplays
//                            .Where(g => g.Code == code && g.Turn == turn)
//                            .ToList()
//                            .ForEach(g => g.IsGetResult = true);
//            _context.SaveChanges();
//        }

//        public PlayerAnswerDto CheckSubmittedLatest(string code, string username)
//        {
//            int turn = GetLatestTurn(code);
//            int cnt = _context.Gameplays.Where(g => g.Code == code && g.Turn == turn && (g.IsGetResult == false || g.IsGetResult == null)).Count();
//            if (cnt == GetPlayerInRoom(code).Count)
//            {
//                Gameplay gameplay = _context.Gameplays.FirstOrDefault(g => g.Code == code && g.Turn == turn && g.Username == username);
//                PlayerAnswerDto playerAnswerDto = new PlayerAnswerDto();
//                //playerAnswerDto.PlayerAnswer = gameplay.PlayerAnswer;
//                playerAnswerDto.Turn = (int)gameplay.Turn;
//                return playerAnswerDto;
//            }
//            else
//            {
//                return null;
//            }
//        }
//    }
//}
