using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Presentation.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterInputModel Register { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Register.Password != Register.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            using (var client = new HttpClient())
            {
                var registerData = new { Register.Username, Register.Password };
                var response = await client.PostAsJsonAsync("https://localhost:7092/api/Authentication/register", registerData);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Login");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return RedirectToPage("/Error", new { errorMessage });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed.");
                    return Page();
                }
            }
        }

        public class RegisterInputModel
        {
            [Required]
            public string Username { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}
