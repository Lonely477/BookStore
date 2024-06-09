using BookStore.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace BookStore.Presentation.Pages.Authors;

public class CreateModel(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : PageModel
{
    [BindProperty]
    public AuthorDto Author { get; set; } = new AuthorDto();

    public async Task<IActionResult> OnPostAsync()
    {
        string? token = httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        string jsonContent = JsonSerializer.Serialize(Author);
        StringContent httpContent = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7092/api/Author", httpContent);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }

        response.EnsureSuccessStatusCode();
        return RedirectToPage("./Index");
    }
}
