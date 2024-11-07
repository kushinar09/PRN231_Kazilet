using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using PRN231_Kazilet_WebApp.Models;
using PRN231_Kazilet_API.Models.Dto;

namespace PRN231_Kazilet_WebApp.Pages.Folders
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private string folderUrl = "http://localhost:5149/api/Folders/";

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<FolderDTO> Folders { get; set; } = new List<FolderDTO>();

        public async Task OnGetAsync(int folderId)
        {
            var apiUrl = $"http://localhost:5149/api/Courses/by-folder/{folderId}";

            // Call the API and deserialize the JSON response into the Courses list
            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Folders = JsonSerializer.Deserialize<List<FolderDTO>>(json);
            }
            else
            {
                // Handle the error as needed (e.g., log or display a message)
                Folders = new List<FolderDTO>();
            }
        }
    }
}
