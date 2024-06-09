using BookStore.Application.Models;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IUserService _userService, IAuthenticationService _authenticationService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        User? user = await _userService.Authenticate(login.Username, login.Password);

        if (user is null)
        {
            return Unauthorized("Username or password you entered is wrong.");
        }

        string token = _authenticationService.GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        User? user = await _userService.Register(registerRequest.Username, registerRequest.Password);

        if (user is null) // if username exists, service returns null and therefore controller returns bad request
        {
            return BadRequest("User already exists.");
        }

        return Created();
    }
}
