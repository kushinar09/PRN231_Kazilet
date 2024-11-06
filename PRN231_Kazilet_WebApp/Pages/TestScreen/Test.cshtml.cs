using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_Kazilet_WebApp.Models.Dto;
using System.Net.Http.Headers;

namespace PRN231_Kazilet_WebApp.Pages.Test
{
    public class TestModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string questionUrl = "http://localhost:7024/odata/Question";

        [BindProperty]
        public List<QuestionDto> QuestionList { get; set; }

        [BindProperty]
        public int Duration { get; set; }

        [BindProperty]
        public int NumOfQues { get; set; }

        public TestModel()
        {
            _httpClient = new HttpClient
            {
                DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") } }
            };
        }

        public async Task OnGet(int id, bool random, int duration, string selectedQuestions, int numOfQues)
        {
            Duration = duration;
            NumOfQues = numOfQues;

            var url = random
                ? $"{questionUrl}/GetRandom/{id}/{numOfQues}"
                : $"{questionUrl}/GetQuestionsByIds?ids={string.Join(",", selectedQuestions.Split(','))}";

            var jsonStr = await _httpClient.GetStringAsync(url);
            JArray jsonArray = JArray.Parse(jsonStr);
            QuestionList = jsonArray.ToObject<List<QuestionDto>>();
        }
    }
}
