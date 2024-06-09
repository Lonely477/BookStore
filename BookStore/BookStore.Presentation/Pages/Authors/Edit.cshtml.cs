using BookStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStore.Presentation.Pages.Authors;

public class EditModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EditModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public AuthorDto Author { get; set; } = new AuthorDto();

    [BindProperty]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Id = id;
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7092/api/Author/{id}");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        Author = JsonSerializer.Deserialize<AuthorDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        }) ?? new AuthorDto();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string jsonContent = JsonSerializer.Serialize(Author);
        StringContent httpContent = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync($"https://localhost:7092/api/Author/{Id}", httpContent);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        return RedirectToPage("./Index");
    }
}
