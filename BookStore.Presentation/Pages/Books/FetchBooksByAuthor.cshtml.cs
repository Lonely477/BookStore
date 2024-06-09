using BookStore.Application.Grpc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookStore.Presentation.Pages.Books;

public class FetchBooksByAuthorModel : PageModel
{
    private readonly Application.Grpc.BookStore.BookStoreClient _grpcClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FetchBooksByAuthorModel(Application.Grpc.BookStore.BookStoreClient grpcClient, IHttpContextAccessor httpContextAccessor)
    {
        _grpcClient = grpcClient;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public string AuthorLastName { get; set; }

    public List<string> BookTitles { get; set; } = new List<string>();


    public async Task<IActionResult> OnGetAsync()
    {
        string? token = _httpContextAccessor.HttpContext!.Session.GetString("JWTToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Error", new { errorMessage = "Unauthorized access." });
        }
        return Page();
    }
    public async Task OnPostAsync()
    {
        var request = new AuthorRequest { LastName = AuthorLastName };
        var response = await _grpcClient.GetBooksByAuthorAsync(request);

        BookTitles = new List<string>(response.Titles);
    }
}
