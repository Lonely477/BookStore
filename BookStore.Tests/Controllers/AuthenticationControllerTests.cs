using BookStore.Application.Controllers;
using BookStore.Application.Models;
using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookStore.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationController = new AuthenticationController(_userServiceMock.Object, _authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithToken()
        {
            // Arrange
            var loginRequest = new LoginRequest("testuser", "password");
            var user = new User { Id = 1, Username = "testuser", PasswordHash = "BPiZbadjt6lpsQKO4wB1aerzpjVIbdqyEdUSyFud+Ps=", Role = "User" };
            var token = "test_token";

            _userServiceMock.Setup(service => service.Authenticate(loginRequest.Username, loginRequest.Password)).ReturnsAsync(user);
            _authenticationServiceMock.Setup(service => service.GenerateJwtToken(user)).Returns(token);

            // Act
            var result = await _authenticationController.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value;
            Assert.NotNull(value);
            Assert.Equal(token, value.GetType().GetProperty("token").GetValue(value, null));
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest("testuser", "wrongpassword");

            _userServiceMock.Setup(service => service.Authenticate(loginRequest.Username, loginRequest.Password)).ReturnsAsync((User)null);

            // Act
            var result = await _authenticationController.Login(loginRequest);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Username or password you entered is wrong.", unauthorizedResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsCreated_WhenUserIsRegistered()
        {
            // Arrange
            var registerRequest = new RegisterRequest("newuser", "user");
            var user = new User { Id = 1, Username = "newuser", PasswordHash = "BPiZbadjt6lpsQKO4wB1aerzpjVIbdqyEdUSyFud+Ps=", Role = "User" };

            _userServiceMock.Setup(service => service.Register(registerRequest.Username, registerRequest.Password)).ReturnsAsync(user);

            // Act
            var result = await _authenticationController.Register(registerRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var registerRequest = new RegisterRequest("existinguser", "testpassword");

            _userServiceMock.Setup(service => service.Register(registerRequest.Username, registerRequest.Password)).ReturnsAsync((User)null);

            // Act
            var result = await _authenticationController.Register(registerRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User already exists.", badRequestResult.Value);
        }
    }
}
