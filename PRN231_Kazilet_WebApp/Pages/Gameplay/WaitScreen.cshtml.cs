using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PRN231_Kazilet_WebApp.Pages.Gameplay
{
    public class WaitScreenModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string GameplayUrl = "https://localhost:7024/api/Gameplay";

        public WaitScreenModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [BindProperty]
        public string Code { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public List<string> Players { get; set; }

        public async Task OnGetAsync(string code, string username)
        {
            Code = code;
            Username = username;
            await Console.Out.WriteLineAsync(code + " " + username);
            HttpResponseMessage response = await _httpClient.GetAsync(GameplayUrl + "/get-players?code=" + code);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                Players = JsonConvert.DeserializeObject<List<string>>(json);
               
            }
        }
    }
}
