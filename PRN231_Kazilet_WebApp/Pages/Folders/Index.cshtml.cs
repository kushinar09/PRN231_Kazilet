using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using PRN231_Kazilet_WebApp.Models;
using PRN231_Kazilet_API.Models.Dto;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace PRN231_Kazilet_WebApp.Pages.Folders
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string folderUrl = "http://localhost:5149/api/Folders/folders/";

        public IndexModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public List<FolderDTO> Folders { get; set; } = new List<FolderDTO>();

        public async Task OnGetAsync(int userId)
        {
            userId = 1;
            string requestUrl = $"{folderUrl}{userId}";

            HttpResponseMessage res = await _httpClient.GetAsync(requestUrl);
            if (res.IsSuccessStatusCode)
            {
                string json = await res.Content.ReadAsStringAsync();
                Folders = JsonConvert.DeserializeObject<List<FolderDTO>>(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error loading folders from API.");
            }
        }
    }
}
