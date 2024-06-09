using BookStore.Presentation.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Presentation.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ClientAuthenticationService _authService;

        public LoginModel(ClientAuthenticationService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginInputModel Login { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string token = await _authService.LoginAsync(Login.Username, Login.Password);
                HttpContext.Session.SetString("JWTToken", token);
                return RedirectToPage("/Index");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, "Login failed. " + ex.Message);
                return Page();
            }
        }

        public class LoginInputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }
    }
}
