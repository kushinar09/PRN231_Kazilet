using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PRN231_Kazilet_WebApp.Models.Dto;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;

namespace PRN231_Kazilet_WebApp.Pages.TestScreen
{
    public class SelectQuestionsScreenModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string questionUrl = "http://localhost:7024/odata/Question";
        public SelectQuestionsScreenModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public IList<QuestionDto> Questions { get; set; } = default!;
        public async Task OnGet()
        {
            HttpResponseMessage m = await _httpClient.GetAsync(questionUrl+"/1");
            string jsonStr = await m.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(jsonStr);
            var list = temp.value;
            Questions = JsonConvert.DeserializeObject<IList<QuestionDto>>(list.ToString());
        }
    }
}
