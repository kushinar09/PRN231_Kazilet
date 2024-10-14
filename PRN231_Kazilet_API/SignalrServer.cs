
using Microsoft.AspNetCore.SignalR;
using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;
using PRN231_Kazilet_API.Services;
using PRN231_Kazilet_API.Utils;

namespace PRN231_Kazilet_API
{
    public class SignalrServer : Hub
    {

        private readonly PRN231_KaziletContext _context;

        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IAuthService _authService;

        private readonly IQuestionService _questionService;

        private readonly IGameplayService _gameplayService;

        public SignalrServer(PRN231_KaziletContext context, IHttpContextAccessor contextAccessor, IAuthService authService, IQuestionService questionService, IGameplayService gameplayService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _authService = authService;
            _questionService = questionService;
            _gameplayService = gameplayService;
        }

        public override async Task OnConnectedAsync()
        {
            var token = _contextAccessor.HttpContext.Request.Query["token"];
            string username = _authService.GetUsernameFromToken(token);
            string code = _authService.CheckGameplayCodeValid(token);
            await Console.Out.WriteLineAsync("Code: " + code);
            if (!string.IsNullOrEmpty(code))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, code);
                await Clients.Group(code).SendAsync("UserJoined", username);
            }
            else
            {
                Context.Abort();

            }
            await base.OnConnectedAsync();
        }

        public async Task GetQuestion()
        {
            var token = _contextAccessor.HttpContext.Request.Query["token"];
            string username = _authService.GetUsernameFromToken(token);
            string code = _authService.CheckGameplayCodeValid(token);
            int courseId = (int)_context.GameplaySettings.FirstOrDefault(c => c.Code == code).CourseId;
            List<QuestionDto> questionDtos = _questionService.GetAllQuestionsByCourse(courseId);
            int questionId = GameplayUtils.GenerateRandom(questionDtos.Count);
            while(_gameplayService.GetQuestionAlreadyAnswer(code).Contains(questionId))
            {
                questionId = GameplayUtils.GenerateRandom(questionDtos.Count);
            }
            QuestionDto questionDto = _questionService.GetById(questionId);
            await Clients.Group(code).SendAsync("GetQuestion", questionDto);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var token = _contextAccessor.HttpContext.Request.Query["token"];
            string username = _authService.GetUsernameFromToken(token);
            string code = _authService.CheckGameplayCodeValid(token);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, code);
            await Clients.Group(code).SendAsync("UserLeft", username);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
