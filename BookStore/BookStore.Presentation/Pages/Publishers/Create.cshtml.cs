using BookStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStore.Presentation.Pages.Publishers;

public class CreateModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public PublisherDto Publisher { get; set; } = new PublisherDto();

    public async Task<IActionResult> OnPostAsync()
    {
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string jsonContent = JsonSerializer.Serialize(Publisher);
        StringContent httpContent = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7092/api/Publisher", httpContent);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        return RedirectToPage("./Index");
    }
}
