using PRN231_Kazilet_API.Models.Dto;
using PRN231_Kazilet_API.Models.Entities;

namespace PRN231_Kazilet_API.Services
{
    public interface IGameplayService
    {
        public string HostGame(int courseId, string username);

        public bool CheckExistCode(string code);

        public string JoinGame(string code, string username);

        public List<string> GetPlayerInRoom(string code);

        public int[] GetQuestionAlreadyAnswer(string code);
    }
}
