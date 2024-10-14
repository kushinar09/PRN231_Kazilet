using Microsoft.AspNetCore.Mvc;
using PRN231_Kazilet_API.Services;
using PRN231_Kazilet_API.Services.Impl;

namespace PRN231_Kazilet_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameplayController : Controller
    {
        private readonly IGameplayService _gameplayService;

        private readonly IAuthService _authService;

        public GameplayController(IGameplayService gameplayService, IAuthService authService)
        {
            _gameplayService = gameplayService;
            _authService = authService;
        }

        [HttpPost]
        [Route("host")]
        public IActionResult HostGame([FromQuery] int courseId, [FromQuery] string username)
        {
            string code = _gameplayService.HostGame(courseId, username);
            return Ok(new
            {
                Code = code,
                Token = _authService.GetGameplayToken(code, username)
            });   
        }

        [HttpGet]
        [Route("find")]
        public IActionResult FindGame([FromQuery] string code)
        {
            if(_gameplayService.CheckExistCode(code))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("join")]
        public IActionResult JoinGame([FromQuery] string code, [FromQuery] string username)
        {
            string token = _gameplayService.JoinGame(code, username);
            if(!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }
            else
            {
                return BadRequest();
            }
        }

    

        [HttpGet]
        [Route("get-players")]
        public IActionResult GetAllPlayersInRoom([FromQuery] string code)
        {
            return Ok(_gameplayService.GetPlayerInRoom(code));
        }

       
    }
}
