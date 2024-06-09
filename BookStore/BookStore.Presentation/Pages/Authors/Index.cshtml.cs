using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStore.Presentation.Pages.Authors;

public class IndexModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IndexModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<Author> Authors { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7092/api/Author");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        Authors = JsonSerializer.Deserialize<List<Author>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        }) ?? new List<Author>();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.DeleteAsync($"https://localhost:7092/api/Author/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();

        return RedirectToPage("./Index");
    }
}
