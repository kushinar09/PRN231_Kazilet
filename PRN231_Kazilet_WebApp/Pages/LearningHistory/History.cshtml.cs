using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PRN231_Kazilet_WebApp.Models.Dto;

namespace PRN231_Kazilet_WebApp.Pages.LearningHistory
{
    public class HistoryModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string learningHistorynUrl = "http://localhost:7024/api/LearningHistory";
        public HistoryModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }
        [BindProperty]
        public int UserId {  get; set; }
        [BindProperty]
        public List<LearningHistoryDto> LearningHistories { get; set; }
        public async Task OnGet(int userId)
        {
            UserId = userId;
            HttpResponseMessage response = await _httpClient.GetAsync($"{learningHistorynUrl}/{userId}");
            string jsonStr = await response.Content.ReadAsStringAsync();
            LearningHistories = JsonConvert.DeserializeObject<List<LearningHistoryDto>>(jsonStr);
        }
    }
}
