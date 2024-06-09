using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStore.Presentation.Pages.Books;

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
    public BookDto Book { get; set; } = new BookDto();

    [BindProperty]
    public int Id { get; set; }

    public List<Author> Authors { get; set; } = new();
    public List<Publisher> Publishers { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Id = id;
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7092/api/Book/{id}");
        HttpResponseMessage authorResponse = await _httpClient.GetAsync("https://localhost:7092/api/Author");
        HttpResponseMessage publisherResponse = await _httpClient.GetAsync("https://localhost:7092/api/Publisher");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            authorResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
            publisherResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        authorResponse.EnsureSuccessStatusCode();
        publisherResponse.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        string authorResponseContent = await authorResponse.Content.ReadAsStringAsync();
        string publisherResponseContent = await publisherResponse.Content.ReadAsStringAsync();

        Book = JsonSerializer.Deserialize<BookDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        }) ?? new BookDto();

        Authors = JsonSerializer.Deserialize<List<Author>>(authorResponseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        }) ?? new List<Author>();

        Publishers = JsonSerializer.Deserialize<List<Publisher>>(publisherResponseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve
        }) ?? new List<Publisher>();

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

        string jsonContent = JsonSerializer.Serialize(Book);
        StringContent httpContent = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync($"https://localhost:7092/api/Book/{Id}", httpContent);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        return RedirectToPage("./Index");
    }
}
