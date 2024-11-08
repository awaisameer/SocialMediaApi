using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

public class AuthController : ControllerBase
{
    private readonly IUserService _userService; // Use IUserService here
    private readonly AuthService _authService;

    public AuthController(IUserService userService, AuthService authService) // Change here
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        // Check if the user object is null
        if (user == null)
        {
            return BadRequest("User data is required");
        }

        // Check if the email is provided
        if (string.IsNullOrEmpty(user.Email))
        {
            return BadRequest("Email is required");
        }

        // Check if the user already exists
        var existingUser = await _userService.FindUserByEmail(user.Email);
        if (existingUser != null)
        {
            return BadRequest("User already exists");
        }

        // Hash the password and save the user
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        await _userService.AddUser(user);

        return Ok("User registered successfully");
    }

    // Login method (modified to accept only email and password)
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        // Find the user by email
        var user = await _userService.FindUserByEmail(loginRequest.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.PasswordHash, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        // Generate JWT token
        var token = _authService.GenerateJwtToken(user);

        return Ok(new { Token = token });
    }
}
